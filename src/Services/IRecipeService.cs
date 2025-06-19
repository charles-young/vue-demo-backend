using RecipeApi.Models;

namespace RecipeApi.Services
{
    public interface IRecipeService
    {
        Task<RecipeResponse> GetRecipesAsync(int? limit = null, int? skip = null);
        Task<Recipe> GetRecipeByIdAsync(int id);
        Task<List<Recipe>> SearchRecipesAsync(string query);
        Task<List<Recipe>> GetRecipesByTagAsync(string tag);
        Task<List<Recipe>> GetRecipesByMealTypeAsync(string mealType);
    }
}