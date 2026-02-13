using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using PopsicleFactoryCo.Models;

namespace PopsicleFactoryCo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    class ErrorController(ILogger<ErrorController> logger) : ControllerBase
    {
        [HttpGet("Error")]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            
            if(exception is null)
            {
                return Problem("An unknown error");

            }
            logger.LogError(exception, "An unhandled exception occurred.");
            return Problem(detail: exception.Message, title: "An error occurred while processing your request.");
        }
    }

}
