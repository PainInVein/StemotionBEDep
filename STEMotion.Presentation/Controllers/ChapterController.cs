using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly IChapterService _chapterService;

        public ChapterController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        [EndpointDescription("API này sẽ lấy tất cả Chapter trong db")]
        [HttpGet]
        public async Task<IActionResult> GetAllChapter()
        {
            var chapters = await _chapterService.GetAllChapter();
            return Ok(chapters);
        }

        [EndpointDescription("API này lấy Chapter theo Id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapterByIdAsync(Guid id)
        {
            var chapter = await _chapterService.GetChapterById(id);
            return Ok(chapter);
        }

        [EndpointDescription("API này để tạo Chapter")]
        [HttpPost]
        public async Task<IActionResult> CreateChapter([FromBody] ChapterRequestDTO createChapterRequest)
        {
            var result = await _chapterService.CreateChapter(createChapterRequest);
            return Ok(result);
        }

        [EndpointDescription("API này để sửa Chapter theo Id")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChapter(Guid id, [FromBody] UpdateChapterRequestDTO updateChapterRequest)
        {
            var result = await _chapterService.UpdateChapter(id, updateChapterRequest);
            return Ok(result);
        }

        [EndpointDescription("API này để xóa Chapter theo Id")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapter(Guid id)
        {
            var result = await _chapterService.DeleteChapter(id);
            return Ok(result);
        }
    }
}
