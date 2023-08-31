using MongoDB.Driver;
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
                var message = error?.Message;
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case ApplicationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case MongoWriteException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        message = "Wrong data for interaction with the company";
                        break;
                    case UserNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UnauthorizedException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case FormatException:
                    case IndexOutOfRangeException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        _logger.LogError(error, message);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                if (response.StatusCode != (int)HttpStatusCode.InternalServerError)
                {
                    var result = JsonSerializer.Serialize(new { message = message });
                    await response.WriteAsync(result);
                }
            }
        }
    }
}
