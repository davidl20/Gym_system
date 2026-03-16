using EvolCep.Dtos;
using System.Net;
using System.Text.Json;

namespace EvolCep.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync (context, ex);
            }
        }

        private async Task HandleExceptionAsync (HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
                KeyNotFoundException => (int)HttpStatusCode.NotFound,           // 404
                InvalidOperationException => (int)HttpStatusCode.BadRequest,   // 400
                _ => (int)HttpStatusCode.InternalServerError                 // 500
            };

            context.Response.StatusCode = statusCode;

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                // Si es un error genérico (500), no mostramos el mensaje técnico en producción
                Message = statusCode == 500 ? "Ocurrió un error inesperado en el servidor." : exception.Message,
                // Solo mostramos el StackTrace si estamos en modo Desarrollo
                Details = _env.IsDevelopment() ? exception.StackTrace : null
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }

    
}
