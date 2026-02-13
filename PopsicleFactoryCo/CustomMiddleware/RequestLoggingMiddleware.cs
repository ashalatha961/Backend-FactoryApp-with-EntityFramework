using System.Collections.Generic;

namespace PopsicleFactoryCo.CustomMiddleware
{
    class RequestLoggingMiddleware
    {
        // Middleware to log incoming HTTP requests
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path}");
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Re-throw the exception to let other middleware handle it if necessary
            }
            return _next(context);
        }
    }
}
