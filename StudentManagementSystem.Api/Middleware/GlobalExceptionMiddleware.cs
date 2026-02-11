using Microsoft.Identity.Client;
using SendGrid.Helpers.Errors.Model;
using StudentManagementSystem.Application.DTOs.ApiResponse;
using StudentManagementSystem.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace StudentManagementSystem.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);// call next middleware
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message = exception.Message;
            switch (exception)
            {
                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    break;

                case DomainException:
                    status = HttpStatusCode.BadRequest;
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    break;
            }

            var response = new ApiResponse<string>
            {
                Success = false,
                Message = message
            };

            var payload = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(payload);
        }
    }
}
