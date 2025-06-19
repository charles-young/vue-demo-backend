namespace RecipeApi.Configuration
{
    public class CorsOptions
    {
        public const string SectionName = "Cors";
        
        public string PolicyName { get; set; } = "RecipeApiCorsPolicy";
        public string[] AllowedOrigins { get; set; } = new[] { "*" };
        public string[] AllowedMethods { get; set; } = new[] { "GET", "POST", "PUT", "DELETE", "OPTIONS" };
        public string[] AllowedHeaders { get; set; } = new[] { "*" };
        public bool AllowCredentials { get; set; } = false;
        public int PreflightMaxAge { get; set; } = 86400; // 24 hours in seconds
    }
}