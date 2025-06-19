namespace RecipeApi.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required List<string> Ingredients { get; set; }
        public required List<string> Instructions { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Servings { get; set; }
        public required string Difficulty { get; set; }
        public required string Cuisine { get; set; }
        public int CaloriesPerServing { get; set; }
        public required List<string> Tags { get; set; }
        public int UserId { get; set; }
        public required string Image { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public required List<string> MealType { get; set; }
    }

    public class RecipeResponse
    {
        public required List<Recipe> Recipes { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}