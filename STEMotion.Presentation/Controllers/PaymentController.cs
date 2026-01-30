using Microsoft.AspNetCore.Mvc;
using PayOS;
using PayOS.Models.V2.PaymentRequests;
using PayOS.Models.Webhooks;
using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Interfaces.ServiceInterfaces;

namespace STEMotion.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly PayOSClient _payOSClient;
        public PaymentController(IPaymentService paymentService, PayOSClient payOSClient)
        {
            _paymentService = paymentService;
            _payOSClient = payOSClient;
        }

        [EndpointDescription("API này check user mua gói chưa")]
        // GET: api/<PaymentController>
        [HttpGet]
        public async Task<IActionResult> GetAllUser(Guid userId)
        {
            var result = await _paymentService.CheckUserPaymentAsync(userId);
            return Ok(ResponseDTO<bool>.Success(result, "User đã mua gói"));
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
        {
            var orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var paymentRequest = new CreatePaymentLinkRequest
            {
                OrderCode = orderCode,
                Amount = 2000,
                Description = "Thanh toán đơn hàng",
                CancelUrl = "https://payment-testing-fe.vercel.app/payment/cancel",
                ReturnUrl = "https://payment-testing-fe.vercel.app/payment/success"
            };

            // Optional but useful
            // request.Items = new List<Item> { new("Sản phẩm", 1, 100000) };

            var response = await _payOSClient.PaymentRequests.CreateAsync(paymentRequest);

            return Ok(new
            {
                checkoutUrl = response.CheckoutUrl,
                paymentLinkId = response.PaymentLinkId,
                orderCode = response.OrderCode
            });
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook([FromBody] Webhook webhookData)
        {
            try
            {
                var verifiedData = await _payOSClient.Webhooks.VerifyAsync(webhookData);
                Console.WriteLine($"Thanh toán thành công: {verifiedData.OrderCode}");
                return Ok("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Webhook không hợp lệ: {ex.Message}");
                return BadRequest("Invalid webhook");
            }
        }

        [HttpPost("payment/{orderCode}/cancel")]
        public async Task<IActionResult> Cancel(long orderCode)
        {
            var result = await _payOSClient.PaymentRequests.CancelAsync(orderCode);
            return Ok(result);
        }

    }
    public class CreatePaymentDto
    {
        public int Amount { get; set; }      // in VND, no decimals
        public string? Description { get; set; }
    }
}
