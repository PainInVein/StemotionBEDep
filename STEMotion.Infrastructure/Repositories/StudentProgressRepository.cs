using Microsoft.EntityFrameworkCore;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using STEMotion.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure.Repositories
{
    public class StudentProgressRepository : GenericRepository<StudentProgress>, IStudentProgressRepository
    {
        public StudentProgressRepository(StemotionContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StudentProgress>> GetProgressByChapterAsync(Guid studentId, Guid chapterId)
        {
            return await FindByCondition(x => x.StudentId == studentId && x.Lesson.ChapterId == chapterId, false)
                         .Include(x => x.Lesson)
                         .ToListAsync();
        }

        public async Task<StudentProgress?> GetProgressByStudentAndLesssonAsync(Guid studentId, Guid lessonId)
        {
            return await FindByCondition(x => x.StudentId == studentId && x.Lesson.LessonId == lessonId, false)
                         .Include(x => x.Lesson)
                         .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<StudentProgress>> GetProgressBySubjectAsync(Guid studentId, Guid subjectId)
        {
            return await FindByCondition(x => x.StudentId == studentId && x.Lesson.Chapter.SubjectId == subjectId, false)
                        .Include(x => x.Lesson)
                                .ThenInclude(l => l.Chapter)
                        .ToListAsync();
        }

        public async Task<IEnumerable<StudentProgress>> GetStudentProgressByStudentIdAsync(Guid studentId)
        {
            return await FindByCondition(x => x.StudentId == studentId, false)
                         .Include(x => x.Lesson)
                                 .ThenInclude(x => x.Chapter)
                                 .ThenInclude(x => x.Subject)
                                 .ThenInclude(x => x.Grade)
                         .ToListAsync();
        }
    }
}
