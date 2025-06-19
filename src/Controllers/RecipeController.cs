using Microsoft.AspNetCore.Mvc;
using RecipeApi.Models;
using RecipeApi.Services;

namespace RecipeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly ILogger<RecipesController> _logger;

        public RecipesController(IRecipeService recipeService, ILogger<RecipesController> logger)
        {
            _recipeService = recipeService;
            _logger = logger;
        }

        /// <summary>
        /// Get all recipes with optional pagination
        /// </summary>
        /// <param name="limit">Number of recipes to return</param>
        /// <param name="skip">Number of recipes to skip</param>
        [HttpGet]
        [ProducesResponseType(typeof(RecipeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecipes([FromQuery] int? limit = null, [FromQuery] int? skip = null)
        {
            try
            {
                var recipes = await _recipeService.GetRecipesAsync(limit, skip);
                return Ok(recipes);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Error retrieving recipes");
                return StatusCode(500, new { error = "Failed to retrieve recipes", message = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific recipe by ID
        /// </summary>
        /// <param name="id">Recipe ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Recipe), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            try
            {
                var recipe = await _recipeService.GetRecipeByIdAsync(id);
                if (recipe == null)
                    return NotFound(new { error = $"Recipe with ID {id} not found" });
                
                return Ok(recipe);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Error retrieving recipe {RecipeId}", id);
                return StatusCode(500, new { error = "Failed to retrieve recipe", message = ex.Message });
            }
        }

        /// <summary>
        /// Search recipes by query
        /// </summary>
        /// <param name="q">Search query</param>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<Recipe>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchRecipes([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest(new { error = "Search query is required" });

            try
            {
                var recipes = await _recipeService.SearchRecipesAsync(q);
                return Ok(recipes);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Error searching recipes with query: {Query}", q);
                return StatusCode(500, new { error = "Failed to search recipes", message = ex.Message });
            }
        }

        /// <summary>
        /// Get recipes by tag
        /// </summary>
        /// <param name="tag">Recipe tag</param>
        [HttpGet("tag/{tag}")]
        [ProducesResponseType(typeof(List<Recipe>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecipesByTag(string tag)
        {
            try
            {
                var recipes = await _recipeService.GetRecipesByTagAsync(tag);
                return Ok(recipes);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Error retrieving recipes by tag: {Tag}", tag);
                return StatusCode(500, new { error = "Failed to retrieve recipes by tag", message = ex.Message });
            }
        }

        /// <summary>
        /// Get recipes by meal type
        /// </summary>
        /// <param name="mealType">Meal type (e.g., breakfast, lunch, dinner)</param>
        [HttpGet("meal-type/{mealType}")]
        [ProducesResponseType(typeof(List<Recipe>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecipesByMealType(string mealType)
        {
            try
            {
                var recipes = await _recipeService.GetRecipesByMealTypeAsync(mealType);
                return Ok(recipes);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Error retrieving recipes by meal type: {MealType}", mealType);
                return StatusCode(500, new { error = "Failed to retrieve recipes by meal type", message = ex.Message });
            }
        }
    }
}