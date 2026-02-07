using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentProgressController : ControllerBase
    {
        private readonly IStudentProgressService _studentProgressService;

        public StudentProgressController(IStudentProgressService studentProgressService)
        {
            _studentProgressService = studentProgressService;
        }

        [HttpGet("lesson/{lessonId}/student/{studentId}")]
        [EndpointDescription("Lấy tiến độ của một lesson cụ thể")]
        public async Task<IActionResult> GetLessonProgress(Guid studentId, Guid lessonId)
        {
            var result = await _studentProgressService.GetLessonProgressAsync(studentId, lessonId);
            
            if (result == null)
            {
                return Ok(ResponseDTO<object>.Success(null, "Lesson chưa được bắt đầu"));
            }

            return Ok(ResponseDTO<LessonProgressResponseDTO>.Success(result, "Lấy tiến độ lesson thành công"));
        }

        [HttpPost("lesson/{lessonId}/student/{studentId}/start")]
        [EndpointDescription("Bắt đầu học một lesson")]
        public async Task<IActionResult> StartLesson(Guid studentId, Guid lessonId)
        {
            var result = await _studentProgressService.StartLessonAsync(studentId, lessonId);
            return Ok(ResponseDTO<LessonProgressResponseDTO>.Success(result, "Bắt đầu lesson thành công"));
        }

        [HttpPut("lesson/{lessonId}/student/{studentId}")]
        [EndpointDescription("Cập nhật tiến độ lesson")]
        public async Task<IActionResult> UpdateLessonProgress(
            Guid studentId, 
            Guid lessonId,
            [FromBody] UpdateProgressRequest request)
        {
            var result = await _studentProgressService.UpdateLessonProgressAsync(
                studentId, lessonId, request.CompletionPercentage, request.IsCompleted);
            
            return Ok(ResponseDTO<LessonProgressResponseDTO>.Success(result, "Cập nhật tiến độ thành công"));
        }

        [HttpGet("chapter/{chapterId}/student/{studentId}")]
        [EndpointDescription("Lấy tiến độ theo chapter")]
        public async Task<IActionResult> GetProgressByChapter(Guid studentId, Guid chapterId)
        {
            var result = await _studentProgressService.GetProgressByChapterAsync(studentId, chapterId);
            return Ok(ResponseDTO<ChapterProgressResponseDTO>.Success(result, "Lấy tiến độ chapter thành công"));
        }

        [HttpGet("subject/{subjectId}/student/{studentId}")]
        [EndpointDescription("Lấy tiến độ theo subject")]
        public async Task<IActionResult> GetProgressBySubject(Guid studentId, Guid subjectId)
        {
            var result = await _studentProgressService.GetProgressBySubjectAsync(studentId, subjectId);
            return Ok(ResponseDTO<SubjectProgressResponseDTO>.Success(result, "Lấy tiến độ subject thành công"));
        }

        [HttpGet("student/{studentId}/overview")]
        [EndpointDescription("Lấy tổng quan tiến độ học tập của học sinh")]
        public async Task<IActionResult> GetStudentOverview(Guid studentId)
        {
            var result = await _studentProgressService.GetStudentProgressOverviewAsync(studentId);
            return Ok(ResponseDTO<StudentProgressOverviewDTO>.Success(result, "Lấy tổng quan thành công"));
        }

        [HttpGet("parent/{parentId}/students")]
        [EndpointDescription("Lấy danh sách học sinh của phụ huynh")]
        public async Task<IActionResult> GetParentStudents(Guid parentId)
        {
            var result = await _studentProgressService.GetParentStudentListAsync(parentId);
            return Ok(ResponseDTO<IEnumerable<ParentStudentListDTO>>.Success(result, "Lấy danh sách thành công"));
        }
    }
}
