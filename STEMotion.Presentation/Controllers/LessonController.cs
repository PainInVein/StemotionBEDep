using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [EndpointDescription("API này sẽ lấy tất cả Lesson trong db")]
        [HttpGet]
        public async Task<IActionResult> GetAllLesson()
        {
            var lessons = await _lessonService.GetAllLesson();
            return Ok(lessons);
        }

        [EndpointDescription("API này lấy Lesson theo Id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonByIdAsync(Guid id)
        {
            var lesson = await _lessonService.GetLessonById(id);
            return Ok(lesson);
        }

        [EndpointDescription("API này để tạo Lesson")]
        [HttpPost]
        public async Task<IActionResult> CreateLesson([FromBody] LessonRequestDTO createLessonRequest)
        {
            var result = await _lessonService.CreateLesson(createLessonRequest);
            return Ok(result);
        }

        [EndpointDescription("API này để sửa Lesson theo Id")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(Guid id, [FromBody] UpdateLessonRequestDTO updateLessonRequest)
        {
            var result = await _lessonService.UpdateLesson(id, updateLessonRequest);
            return Ok(result);
        }

        [EndpointDescription("API này để xóa Lesson theo Id")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(Guid id)
        {
            var result = await _lessonService.DeleteLesson(id);
            return Ok(result);
        }
    }
}
