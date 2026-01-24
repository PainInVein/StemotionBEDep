using STEMotion.Application.DTO.ResponseDTOs;
using STEMotion.Application.Exceptions;

namespace STEMotion.Presentation.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var response = ResponseDTO<object>.Fail(
                    ex.Message,
                    ex.ErrorCode
                );

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;

                var response = ResponseDTO<object>.Fail(
                    "Lỗi hệ thống",
                    "INTERNAL_SERVER_ERROR"
                );

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
