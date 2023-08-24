using System;
using System.Net;
using System.Text.Json;
using VC.Helpers.Exceptions;

namespace VC.Data
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case FormatException:
                    case IndexOutOfRangeException:
                    case AppException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case UserNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UnauthorizedException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:
                        _logger.LogError(error, error.Message);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                if (response.StatusCode != (int)HttpStatusCode.InternalServerError)
                {
                    var result = JsonSerializer.Serialize(new { message = error?.Message });
                    await response.WriteAsync(result);
                }
            }
        }
    }
}
