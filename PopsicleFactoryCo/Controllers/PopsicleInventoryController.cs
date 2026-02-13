using FluentValidation;
using FluentValidation.Results;
using PopsicleFactoryCo.Data;
using PopsicleFactoryCo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

/* For conventional routing, remove [Route] Attribute from both Controller level and action method level. We also need to remove [ApiController] attribute,
which is decorated with the Controller class. This is because [ApiController] will force action method to use Route attribute. Scenarios like MVC views (using
conventional routing) and Web API controllers (attribute routing),we can use both Conventional and Attribute-Based Routing in an ASP.NET Core Web API Application.
Action method decorated with Route Attribute will use Attribute Routing, while the action method without Route Attribute will use Conventional Based Routing*/

namespace PopsicleFactoryCo.Controllers
{
    [ApiController] // Indicates that this class is an API controller, which means it will handle HTTP requests and responsesss
    //[Route("api/[controller]")] Sets the base route for all actions in this controller to be prefixed with "api/PopsicleInventory"
    [Route("[controller]")] // Sets the base route for all actions in this controller to be prefixed with "PopsicleInventory"
    public class PopsicleInventoryController : ControllerBase
    {
        private readonly IPopsicleInventory _inventoryRepository;
        private readonly IValidator<PopsicleModel> _validator;
        private readonly ILogger<PopsicleInventoryController> _logger;

        public PopsicleInventoryController(IPopsicleInventory inventoryRepository, IValidator<PopsicleModel> validator, ILogger<PopsicleInventoryController> logger)
        {
            this._inventoryRepository = inventoryRepository;
            this._validator = validator;
            this._logger = logger;
        }

        [Tags("CreateNewPopsicle")]
        //[Route("CreateNewPopsicle", Name = "CreateNewPopsicle")]
        //[Authorize(Roles = "Administrator")] -- Authorization attribute to restrict access to users with the "Administrator" role.
        //[EnableCors("MyAllowSpecificOrigins")]
        [HttpPost(Name = "CreateNewPopsicle")]
        public async Task<IActionResult> CreateNewPopsicle([FromBody] PopsicleModel popsicle)
        {
            try
            {
                if (popsicle == null)
                {
                    return BadRequest("Popsicle data is required");
                }

                // Validate the popsicle model using FluentValidation library
                ValidationResult result = await _validator.ValidateAsync(popsicle);

                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                var createdPopsicle = await _inventoryRepository.CreatePopsicleAsync(popsicle);
                return CreatedAtAction(nameof(GetPopsicleWithId), new { id = createdPopsicle.Id }, createdPopsicle);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "An error occurred while creating a new popsicle.");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Tags("GetPopsicleData {All} / {Based on ID} / {SearchTerm}")]
        [HttpGet(Name = "GetListOfAllPopsicles")]
        //[ActionName("PopsicleLists")] // This allows the method to be called without a specific action name    
        public async Task<ActionResult<IEnumerable<PopsicleModel>>> GetListOfAllPopsicles()
        {
            //replacing IActionResult with <ActionResult<IEnumerable<PopsicleModel>>> 
            try
            {
                var popsicles = await _inventoryRepository.GetListOfAllPopsicles();
                return Ok(popsicles);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Tags("GetPopsicleData {All} / {Based on ID} / {SearchTerm}")]
        [HttpGet("{id:int}", Name = "GetPopsicleWithId")]
        public async Task<IActionResult> GetPopsicleWithId(int id)
        {
            try 
            {
                var popsicleforGivenId = await _inventoryRepository.GetPopsicleByIdAsync(id);

                // Check if the popsicle exists
                if (popsicleforGivenId == null)
                {
                    return NotFound();
                }

                // Use AutoMapper for mapping and hide some of the data if needed
                //var popsicleDto = _mapper.Map<PopsicleDto>(popsicle);
                return Ok(popsicleforGivenId);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Tags("GetPopsicleData {All} / {Based on ID} / {SearchTerm}")]
        [HttpGet("search/{searchItem}", Name = "SearchForPopsicles")]
        public async Task<IActionResult> SearchForPopsicles(string searchItem)
        {
            try
            {
                if (string.IsNullOrEmpty(searchItem))
                {
                    return BadRequest(error: "Search item cannot be null or empty");
                }

                var popsicles = await _inventoryRepository.SearchForPopsiclesAsync(searchItem);

                if (!popsicles.Any())
                {
                    return NotFound();
                }
                return Ok(popsicles);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [Tags("UpdateExistingPopsicleData")]
        [HttpPut("{id:int}", Name = "UpdatePopsicle")]
        public async Task<IActionResult> UpdatePopsicle(int id, [FromBody] PopsicleModel popsicle)
        {
            try
            {
                if (popsicle == null || popsicle.Id != id)
                {
                    return BadRequest("Invalid popsicle data");
                }

                ValidationResult result = await _validator.ValidateAsync(popsicle);

                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                var existingPopsicle = await _inventoryRepository.GetPopsicleByIdAsync(id);

                if (existingPopsicle == null)
                {
                    return NotFound();
                }
                var updatedPopsicle = await _inventoryRepository.UpdatePopsicleAsync(popsicle);
                return Ok(updatedPopsicle);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [Tags("DeleteExistingPopsicle")]
        [HttpDelete("{id:int}", Name = "Delete Popsicle")]
        public async Task<IActionResult> DeletePopsicle(int id)
        {
            try
            {
                var isDeleted = await _inventoryRepository.DeletePopsicleAsync(id);
                if (!isDeleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}