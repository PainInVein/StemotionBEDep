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
        public Task<ChapterProgressDTO> GetChapterProgressAsync(Guid parentId, Guid studentId, Guid chapterId)
        {
            throw new NotImplementedException();
        }

        public Task<LessonProgressDTO> GetLessonProgressAsync(Guid parentId, Guid studentId, Guid lessonId)
        {
            throw new NotImplementedException();
        }

        public Task<ParentDashboardDTO> GetParentDashboardAsync(Guid parentId)
        {
            throw new NotImplementedException();
        }

        public Task<StudentOverallProgressResponseDTO> GetStudentOverallProgressAsync(Guid parentId, Guid studentId)
        {
            throw new NotImplementedException();
        }

        public Task<SubjectProgressDTO> GetSubjectProgressAsync(Guid parentId, Guid studentId, Guid subjectId)
        {
            throw new NotImplementedException();
        }
    }
}
