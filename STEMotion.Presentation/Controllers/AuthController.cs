using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Application.Services;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;


        public AuthController(IOtpService otpService, IUnitOfWork unitOfWork, IUserService userService, IConfiguration configuration)
        {
            _otpService = otpService;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _configuration = configuration;
        }

        [EndpointDescription("API này đăng nhập User")]
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var result = await _userService.LoginUser(loginRequestDTO);
            return Ok(result);
        }

        [HttpGet("google-login")]
        [EndpointDescription("API này để login Google")]
        public IActionResult GoogleLogin([FromQuery] string role = "student")
        {
            var redirectUri = _configuration["Authentication:Google:RedirectUri"];
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUri
            };
            properties.Items.Add("RoleName", role);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Google authentication failed" });
            }

            result.Properties.Items.TryGetValue("RoleName", out var roleName);
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;

            var googleRequestDTO = new GoogleRequestDTO
            {
                Email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                FirstName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                LastName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                AvatarUrl = claims?.FirstOrDefault(c => c.Type == "picture")?.Value,
                RoleName = roleName
            };
            if (string.IsNullOrEmpty(googleRequestDTO.Email))
            {
                return BadRequest(new { message = "Email not found in Google account" });
            }

            var response = await _userService.AuthenticateGoogleUserAsync(googleRequestDTO);

            if (!response.IsSuccess)
            {
                return Redirect($"http://localhost:5173/login-error?message={response.Message}");
            }

            //var token = response.Result;
            //return Redirect($"http://localhost:5173/login-success?token={token}");
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddHours(24)
            };

            Response.Cookies.Append("accessToken", response.Result, cookieOptions);
            return Redirect("http://localhost:5173");
        }

        [HttpPost("register/send-otp")]
        [EndpointDescription("Bước 1: Gửi mã OTP để xác thực email khi đăng ký (FE phải gửi roleName: student hoặc parent)")]
        public async Task<IActionResult> SendRegistrationOtp([FromBody] CreateUserRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate roleName
            if (string.IsNullOrEmpty(request.RoleName))
                return BadRequest(new ResponseDTO<string>("RoleName là bắt buộc (student hoặc parent)", false, null));

            var result = await _userService.RequestRegistrationOtpAsync(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("register/verify-otp")]
        [EndpointDescription("Bước 2: Xác thực OTP và tạo tài khoản")]
        public async Task<IActionResult> VerifyOtpAndRegister([FromBody] OtpVerificationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.VerifyOtpAndRegisterAsync(request.Email, request.OtpCode);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("password/send-otp")]
        [EndpointDescription("Bước 1: Gửi mã OTP để đổi mật khẩu")]
        public async Task<IActionResult> SendPasswordResetOtp([FromBody] OtpRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RequestPasswordResetOtpAsync(request.Email);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [HttpPost("password/reset")]
        [EndpointDescription("Bước 2: Xác thực OTP và đổi mật khẩu")]
        public async Task<IActionResult> ResetPassword([FromBody] ChangePasswordRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user is null)
                return NotFound(new ResponseDTO<bool>("Không tìm thấy User.", false, false));

            var result = await _userService.ChangePassword(user.UserId, request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}

