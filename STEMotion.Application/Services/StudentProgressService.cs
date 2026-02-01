using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Services
{
    public class StudentProgressService : IStudentProgressService
    {
        public Task<ChapterProgressResponseDTO> GetChapterProgressAsync(Guid parentId, Guid studentId, Guid chapterId)
        {
            throw new NotImplementedException();
        }

        public Task<LessonProgressResponseDTO> GetLessonProgressAsync(Guid parentId, Guid studentId, Guid lessonId)
        {
            throw new NotImplementedException();
        }

        public Task<ParentStudentListDTO> GetParentDashboardAsync(Guid parentId)
        {
            throw new NotImplementedException();
        }

        public Task<StudentProgressOverviewDTO> GetStudentOverallProgressAsync(Guid parentId, Guid studentId)
        {
            throw new NotImplementedException();
        }

        public Task<SubjectProgressResponseDTO> GetSubjectProgressAsync(Guid parentId, Guid studentId, Guid subjectId)
        {
            throw new NotImplementedException();
        }
    }
}
