using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.RepositoryInterfaces
{
    public interface IStudentProgressRepository : IGenericRepository<StudentProgress>
    {
        Task<IEnumerable<StudentProgress>> GetStudentProgressByStudentIdAsync(Guid studentId);
        Task<StudentProgress?> GetProgressByStudentAndLesssonAsync(Guid studentId, Guid lessonId);
        Task<IEnumerable<StudentProgress>> GetProgressByChapterAsync(Guid studentId, Guid chapterId);
        Task<IEnumerable<StudentProgress>> GetProgressBySubjectAsync(Guid studentId, Guid subjectId);
        Task<bool> CanParentAccessStudentProgressAsync(Guid parentId, Guid studentId);
    }
}
