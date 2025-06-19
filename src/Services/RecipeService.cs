using System.Text.Json;
using RecipeApi.Models;

namespace RecipeApi.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RecipeService> _logger;
        private readonly string _baseUrl = "https://dummyjson.com/recipes";
        private readonly JsonSerializerOptions _jsonOptions;

        public RecipeService(HttpClient httpClient, ILogger<RecipeService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<RecipeResponse> GetRecipesAsync(int? limit = null, int? skip = null)
        {
            try
            {
                var url = _baseUrl;
                var queryParams = new List<string>();
                
                if (limit.HasValue)
                    queryParams.Add($"limit={limit}");
                if (skip.HasValue)
                    queryParams.Add($"skip={skip}");
                
                if (queryParams.Any())
                    url += "?" + string.Join("&", queryParams);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<RecipeResponse>(json, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching recipes from external API");
                throw new ServiceException("Failed to fetch recipes", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing recipe response");
                throw new ServiceException("Invalid response format", ex);
            }
        }

        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Recipe>(json, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching recipe {RecipeId} from external API", id);
                throw new ServiceException($"Failed to fetch recipe with ID {id}", ex);
            }
        }

        public async Task<List<Recipe>> SearchRecipesAsync(string query)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/search?q={Uri.EscapeDataString(query)}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RecipeResponse>(json, _jsonOptions);
                return result?.Recipes ?? new List<Recipe>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error searching recipes with query: {Query}", query);
                throw new ServiceException($"Failed to search recipes", ex);
            }
        }

        public async Task<List<Recipe>> GetRecipesByTagAsync(string tag)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/tag/{Uri.EscapeDataString(tag)}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RecipeResponse>(json, _jsonOptions);
                return result?.Recipes ?? new List<Recipe>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching recipes by tag: {Tag}", tag);
                throw new ServiceException($"Failed to fetch recipes by tag", ex);
            }
        }

        public async Task<List<Recipe>> GetRecipesByMealTypeAsync(string mealType)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/meal-type/{Uri.EscapeDataString(mealType)}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RecipeResponse>(json, _jsonOptions);
                return result?.Recipes ?? new List<Recipe>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching recipes by meal type: {MealType}", mealType);
                throw new ServiceException($"Failed to fetch recipes by meal type", ex);
            }
        }
    }

    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}