using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace STEMotion.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
        }

        [EndpointDescription("API này lấy tất cả User trong db")]
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [EndpointDescription("API này lấy User theo Id")]
        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }

        [EndpointDescription("API này tạo mới User")]
        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO createUserRequestDTO)
        {
            var result = await _userService.CreateUser(createUserRequestDTO);
            return Ok(result);
        }

        [EndpointDescription("API này cập nhật User theo Id")]
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequestDTO updateUserRequestDTO)
        {
            var result = await _userService.UpdateUser(id, updateUserRequestDTO);
            return Ok(result);
        }

        [EndpointDescription("API này xóa User theo Id")]
        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUser(id);
            return Ok(result);
        }
    }
}