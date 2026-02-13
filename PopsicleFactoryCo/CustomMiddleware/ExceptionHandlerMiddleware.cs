using System.Collections.Generic;

namespace PopsicleFactoryCo.CustomMiddleware
{
    class ExceptionHandlerMiddleware
    {
        //This middleware would catch unhandled exceptions that occur during request processing,
        //log the details of the exception, and return a standardized error response to the client.
        //This helps improve the robustness of the application by ensuring that unexpected errors are managed gracefully.

        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Call the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"An unhandled exception occurred: {ex.Message}");
                
                // Return a standardized error response to the client
                context.Response.StatusCode = 500; // Internal Server Error
                context.Response.ContentType = "application/json";
                var errorResponse = new { message = "An unexpected error occurred. Please try again later." };
                await context.Response.WriteAsJsonAsync(errorResponse);

            }
        }
    }
    
}
