using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieBackendAPI.Business.Exceptions;
using MovieBackendAPI.Domain.Const;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieBackendAPI.Middleware
{
    internal class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
        public ErrorHandlerMiddleware(RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException error)
            {
                if (error.ValidationErrors != null) error.StatusMessage = error.FormattedError;

                _logger.LogError(error, error.Message);
                var response = context.Response;
                response.ContentType = "application/json";

                Result<string> serviceResponse = new Result<string>()
                {
                    StatusCode = error.StatusCode,
                    StatusMessage = error.StatusMessage,

                };

                response.StatusCode = (int)HttpStatusCode.BadRequest;
                var result = JsonSerializer.Serialize(serviceResponse);

                await response.WriteAsync(result);

            }
            catch (Exception error)
            {
                _logger.LogError(error, error.Message);
                var response = context.Response;
                response.ContentType = "application/json";

                Result<string> serviceResponse = new Result<string>
                {
                    StatusCode = ResponseCode.GENERIC_EXCEPTION,
                };

                if (_env.IsDevelopment() || _env.IsUat() || _env.IsLocal())
                {
                    serviceResponse.StatusMessage = error.Message;
                }
                else
                {
                    serviceResponse.StatusMessage = "An error occured";
                }


                // unhandled error
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var result = JsonSerializer.Serialize(serviceResponse);

                await response.WriteAsync(result);
            }
        }
    }
}
