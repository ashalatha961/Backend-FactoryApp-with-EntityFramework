using System.Collections.Generic;

namespace PopsicleFactoryCo.CustomMiddleware
{
    class RequestTimingMiddleware
    {
        //This middleware logs details about incoming requests and their processing time,
        //which is crucial for monitoring application health and identifying performance bottlenecks.
        private readonly RequestDelegate _next;

        public RequestTimingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew(); // Start timing
                await _next(context); // Call the next middleware in the pipeline
                stopwatch.Stop();
                var processingTime = stopwatch.ElapsedMilliseconds;

                // Log the request details and processing time
                Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path} processed in {processingTime} ms");
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Re-throw the exception to let other middleware handle it if necessary
            }
        }
    }
}
