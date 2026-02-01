using STEMotion.Application.DTO.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IStudentProgressService
    {
        /// <summary>
        /// Lấy dashboard tổng quan cho phụ huynh với tất cả con
        /// </summary>
        /// <param name="parentId">ID của phụ huynh</param>
        /// <returns>Dashboard với thông tin tổng quan của tất cả con</returns>
        Task<ParentStudentListDTO> GetParentDashboardAsync(Guid parentId);

        /// <summary>
        /// Lấy tổng quan tiến trình học chi tiết của một học sinh
        /// </summary>
        /// <param name="parentId">ID của phụ huynh</param>
        /// <param name="studentId">ID của học sinh</param>
        /// <returns>Tổng quan tiến trình học với chi tiết từng môn</returns>
        Task<StudentProgressOverviewDTO> GetStudentOverallProgressAsync(Guid parentId, Guid studentId);

        /// <summary>
        /// Lấy tiến trình học theo môn học
        /// </summary>
        /// <param name="parentId">ID của phụ huynh</param>
        /// <param name="studentId">ID của học sinh</param>
        /// <param name="subjectId">ID của môn học</param>
        /// <returns>Chi tiết tiến trình môn học với danh sách chapter</returns>
        Task<SubjectProgressResponseDTO> GetSubjectProgressAsync(Guid parentId, Guid studentId, Guid subjectId);

        /// <summary>
        /// Lấy tiến trình học theo chương
        /// </summary>
        /// <param name="parentId">ID của phụ huynh</param>
        /// <param name="studentId">ID của học sinh</param>
        /// <param name="chapterId">ID của chương</param>
        /// <returns>Chi tiết tiến trình chương với danh sách lesson</returns>
        Task<ChapterProgressResponseDTO> GetChapterProgressAsync(Guid parentId, Guid studentId, Guid chapterId);

        /// <summary>
        /// Lấy tiến trình học theo bài học
        /// </summary>
        /// <param name="parentId">ID của phụ huynh</param>
        /// <param name="studentId">ID của học sinh</param>
        /// <param name="lessonId">ID của bài học</param>
        /// <returns>Chi tiết tiến trình của bài học</returns>
        Task<LessonProgressResponseDTO> GetLessonProgressAsync(Guid parentId, Guid studentId, Guid lessonId);
    }
}
