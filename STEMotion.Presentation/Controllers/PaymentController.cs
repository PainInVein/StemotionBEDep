using Microsoft.AspNetCore.Mvc;
using STEMotion.Application.DTO.RequestDTOs;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Application.Services;

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [EndpointDescription("API này check user mua gói chưa")]
        // GET: api/<PaymentController>
        [HttpGet]
        public async Task<IActionResult> GetAllUser(Guid userId)
        {
            var result = await _paymentService.CheckUserPaymentAsync(userId);
            return Ok(ResponseDTO<bool>.Success(result, "User đã mua gói"));
        }
    }
}
