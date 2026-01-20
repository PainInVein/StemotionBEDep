using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [EndpointDescription("API này sẽ lấy tất cả Subject trong db")]
        [HttpGet]
        public async Task<IActionResult> GetAllSubject()
        {
            var subjects = await _subjectService.GetAllSubject();
            return Ok(subjects);
        }

        [EndpointDescription("API này lấy Subject theo Id")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectByIdAsync(Guid id)
        {
            var subject = await _subjectService.GetSubjectById(id);
            return Ok(subject);
        }

        [EndpointDescription("API này để tạo Subject")]
        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectRequestDTO createSubjectRequest)
        {
            var result = await _subjectService.CreateSubject(createSubjectRequest);
            return Ok(result);
        }

        [EndpointDescription("API này để sửa Subject theo Id")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(Guid id, [FromBody] UpdateSubjectRequestDTO updateSubjectRequest)
        {
            var result = await _subjectService.UpdateSubject(id, updateSubjectRequest);
            return Ok(result);
        }

        [EndpointDescription("API này để xóa Subject theo Id")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            var result = await _subjectService.DeleteSubject(id);
            return Ok(result);
        }
    }
}
