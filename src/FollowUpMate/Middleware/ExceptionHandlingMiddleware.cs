using FollowUpMate.Application.Logging;
using FollowUpMate.Infrastructure.Logging;
using System;
using System.Net;
using System.Text.Json;

namespace FollowUpMate.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppLogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, IAppLogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var traceId = context.TraceIdentifier;
                _logger.LogError(ex,
                    $"Unhandled exception.{Environment.NewLine}" +
                    $"TraceId: {traceId}, Path: {context.Request.Path}, Method: {context.Request.Method}{Environment.NewLine}");
                var response = new
                {
                    success = false,
                    statusCode = context.Response.StatusCode,
                    error = new
                    {
                        type = ex.GetType().Name,
                        message = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred. Please try again later.",
                        traceId = traceId,
                        path = context.Request.Path,
                        method = context.Request.Method,
                        timeStamp = DateTime.UtcNow
                    }
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }

        }
    }
}
