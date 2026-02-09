using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Exceptions;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Services
{
    public class StudentProgressService : IStudentProgressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentProgressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<LessonProgressResponseDTO> GetLessonProgressAsync(Guid studentId, Guid lessonId)
        {
            var progress = await _unitOfWork.StudentProgressRepository
                .GetProgressByStudentAndLesssonAsync(studentId, lessonId);

            if (progress == null)
            {
                throw new AlreadyExistsException(studentId.ToString(), lessonId.ToString());
            }

            return _mapper.Map<LessonProgressResponseDTO>(progress);
        }

        public async Task<LessonProgressResponseDTO> StartLessonAsync(Guid studentId, Guid lessonId)
        {
            var existingProgress = await _unitOfWork.StudentProgressRepository
                .GetProgressByStudentAndLesssonAsync(studentId, lessonId);

            if (existingProgress != null)
            {
                existingProgress.LastAccessedAt = DateTime.UtcNow;
                
                if (existingProgress.StartedAt == null)
                {
                    existingProgress.StartedAt = DateTime.UtcNow;
                    existingProgress.Status = "in_progress";
                }

                _unitOfWork.StudentProgressRepository.Update(existingProgress);
                await _unitOfWork.SaveChangesAsync();
                
                return _mapper.Map<LessonProgressResponseDTO>(existingProgress);
            }

            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(lessonId);
            if (lesson == null)
                throw new NotFoundException("Lesson", lessonId);

            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            if (student == null)
                throw new NotFoundException("Student", studentId);

            var newProgress = new StudentProgress
            {
                StudentId = studentId,
                LessonId = lessonId,
                StartedAt = DateTime.UtcNow,
                LastAccessedAt = DateTime.UtcNow,
                Status = "in_progress",
                CompletionPercentage = 0,
                IsCompleted = false
            };

            await _unitOfWork.StudentProgressRepository.CreateAsync(newProgress);
            await _unitOfWork.SaveChangesAsync();

            var createdProgress = await _unitOfWork.StudentProgressRepository
                .GetProgressByStudentAndLesssonAsync(studentId, lessonId);

            return _mapper.Map<LessonProgressResponseDTO>(createdProgress);
        }
    
        public async Task<LessonProgressResponseDTO> UpdateLessonProgressAsync(
            Guid studentId, Guid lessonId, int completionPercentage, bool isCompleted)
        {
            if (completionPercentage < 0 || completionPercentage > 100)
            {
                throw new BadRequestException("CompletionPercentage phải từ 0 đến 100");
            }

            var progress = await _unitOfWork.StudentProgressRepository
                .GetProgressByStudentAndLesssonAsync(studentId, lessonId);

            if (progress == null)
            {
                await StartLessonAsync(studentId, lessonId);
                progress = await _unitOfWork.StudentProgressRepository
                    .GetProgressByStudentAndLesssonAsync(studentId, lessonId);
            }

            progress.CompletionPercentage = completionPercentage;
            progress.IsCompleted = isCompleted;
            progress.LastAccessedAt = DateTime.UtcNow;

            if (isCompleted && progress.CompletedAt == null)
            {
                progress.CompletedAt = DateTime.UtcNow;
                progress.Status = "completed";
                progress.CompletionPercentage = 100;
            }
            else if (isCompleted)
            {
                progress.Status = "completed";
                progress.CompletionPercentage = 100;
            }
            else
            {
                progress.Status = completionPercentage > 0 ? "in_progress" : "in_progress";
            }

            _unitOfWork.StudentProgressRepository.Update(progress);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LessonProgressResponseDTO>(progress);
        }
        public async Task<ChapterProgressResponseDTO> GetProgressByChapterAsync(Guid studentId, Guid chapterId)
        {
            var chapter = await _unitOfWork.ChapterRepository.GetByIdAsync(chapterId);
            if (chapter == null)
                throw new NotFoundException("Chapter", chapterId);

            var allLessons = await _unitOfWork.LessonRepository
                .FindByCondition(l => l.ChapterId == chapterId, false)
                .ToListAsync();

            if (allLessons.Count == 0)
            {
                return new ChapterProgressResponseDTO
                {
                    ChapterId = chapter.ChapterId,
                    ChapterName = chapter.ChapterName,
                    TotalLessons = 0,
                    CompletedLessons = 0,
                    CompletionPercentage = 0,
                    LessonProgress = new List<LessonProgressResponseDTO>()
                };
            }

            var progressList = await _unitOfWork.StudentProgressRepository
                .GetProgressByChapterAsync(studentId, chapterId);

            var lessonProgressDTOs = allLessons.Select(lesson =>
            {
                var progress = progressList.FirstOrDefault(p => p.LessonId == lesson.LessonId);
                
                if (progress == null)
                {
                    return new LessonProgressResponseDTO
                    {
                        LessonId = lesson.LessonId,
                        LessonName = lesson.LessonName,
                        IsCompleted = false,
                        CompletionPercentage = 0,
                        EstimatedTime = lesson.EstimatedTime
                    };
                }

                return _mapper.Map<LessonProgressResponseDTO>(progress);
            }).ToList();

            int completedCount = lessonProgressDTOs.Count(l => l.IsCompleted);
            double avgCompletion = lessonProgressDTOs.Average(l => l.CompletionPercentage);

            return new ChapterProgressResponseDTO
            {
                ChapterId = chapter.ChapterId,
                ChapterName = chapter.ChapterName,
                TotalLessons = allLessons.Count,
                CompletedLessons = completedCount,
                CompletionPercentage = Math.Round(avgCompletion, 2),
                LessonProgress = lessonProgressDTOs
            };
        }
        public async Task<SubjectProgressResponseDTO> GetProgressBySubjectAsync(Guid studentId, Guid subjectId)
        {
            var subject = await _unitOfWork.SubjectRepository.GetByIdAsync(subjectId);
            if (subject == null)
                throw new NotFoundException("Subject", subjectId);

            var chapters = await _unitOfWork.ChapterRepository
                .FindByCondition(c => c.SubjectId == subjectId, false)
                .ToListAsync();

            if (chapters.Count == 0)
            {
                return new SubjectProgressResponseDTO
                {
                    SubjectId = subject.SubjectId,
                    SubjectName = subject.SubjectName,
                    TotalChapters = 0,
                    TotalLessons = 0,
                    CompletedChapters = 0,
                    CompletedLessons = 0,
                    CompletionPercentage = 0,
                    ChapterProgress = new List<ChapterProgressResponseDTO>()
                };
            }

            var chapterProgressList = new List<ChapterProgressResponseDTO>();

            foreach (var chapter in chapters)
            {
                var chapterProgress = await GetProgressByChapterAsync(studentId, chapter.ChapterId);
                chapterProgressList.Add(chapterProgress);
            }

            int totalLessons = chapterProgressList.Sum(c => c.TotalLessons);
            int completedLessons = chapterProgressList.Sum(c => c.CompletedLessons);
            int completedChapters = chapterProgressList.Count(c => c.CompletionPercentage == 100);

            double completionPercentage = totalLessons > 0
                ? Math.Round((double)completedLessons / totalLessons * 100, 2)
                : 0;

            return new SubjectProgressResponseDTO
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                TotalChapters = chapters.Count,
                TotalLessons = totalLessons,
                CompletedChapters = completedChapters,
                CompletedLessons = completedLessons,
                CompletionPercentage = completionPercentage,
                ChapterProgress = chapterProgressList
            };
        }
        public async Task<StudentProgressOverviewDTO> GetStudentProgressOverviewAsync(Guid studentId)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(studentId);
            if (student == null)
                throw new NotFoundException("Student", studentId);

            var allSubjects = student.GradeLevel.HasValue
                ? await _unitOfWork.SubjectRepository
                    .FindByCondition(s => s.Grade.GradeLevel == student.GradeLevel.Value, false)
                    .ToListAsync()
                : await _unitOfWork.SubjectRepository
                    .FindByCondition(s => true, false)
                    .ToListAsync();

            var subjectProgressList = new List<SubjectProgressResponseDTO>();

            foreach (var subject in allSubjects)
            {
                var subjectProgress = await GetProgressBySubjectAsync(studentId, subject.SubjectId);
                subjectProgressList.Add(subjectProgress);
            }

            int totalLessons = subjectProgressList.Sum(s => s.TotalLessons);
            int completedLessons = subjectProgressList.Sum(s => s.CompletedLessons);

            var allProgress = await _unitOfWork.StudentProgressRepository
                .GetStudentProgressByStudentIdAsync(studentId);
            
            var lastActivity = allProgress.Any() 
                ? allProgress.Max(p => p.LastAccessedAt) 
                : null;

            int overallCompletion = totalLessons > 0
                ? (int)Math.Round((double)completedLessons / totalLessons * 100)
                : 0;

            return new StudentProgressOverviewDTO
            {
                StudentId = student.StudentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                GradeLevel = student.GradeLevel ?? 0,
                TotalSubjects = allSubjects.Count,
                TotalChapters = subjectProgressList.Sum(s => s.TotalChapters),
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                OverallCompletionPercentage = overallCompletion,
                LastActivityDate = lastActivity,
                Subjects = subjectProgressList
            };
        }

        public async Task<IEnumerable<ParentStudentListDTO>> GetParentStudentListAsync(Guid parentId)
        {
            var parent = await _unitOfWork.UserRepository.GetByIdAsync(parentId);
            if (parent == null)
                throw new NotFoundException("Parent", parentId);

            // Lấy danh sách students trực tiếp từ Student table theo ParentId
            var students = await _unitOfWork.StudentRepository.GetStudentsByParentIdAsync(parentId);

            if (students == null || !students.Any())
            {
                return new List<ParentStudentListDTO>(); // Không có học sinh nào
            }

            var result = new List<ParentStudentListDTO>();

            foreach (var student in students)
            {
                var overview = await GetStudentProgressOverviewAsync(student.StudentId);

                result.Add(new ParentStudentListDTO
                {
                    StudentId = student.StudentId,
                    StudentName = $"{student.FirstName} {student.LastName}",
                    GradeLevel = student.GradeLevel ?? 0,
                    AvatarUrl = student.AvatarUrl,
                    OverallProgress = overview.OverallCompletionPercentage,
                    LastActivityDate = overview.LastActivityDate
                });
            }

            return result;
        }

        public async Task<bool> ValidateParentAccessAsync(Guid parentId, Guid studentId)
        {
            return await _unitOfWork.StudentProgressRepository.CanParentAccessStudentProgressAsync(parentId, studentId);
        }
    }
}
