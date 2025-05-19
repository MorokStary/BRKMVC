namespace FinTrack.Api.Contracts.Analysis
{
    public class RecommendationResponse
    {
        public string CategoryName { get; set; }
        public string Message { get; set; }
        public RecommendationSeverity Severity { get; set; }
        public decimal? SuggestedLimit { get; set; }
    }

    public enum RecommendationSeverity
    {
        Low,
        Medium,
        High
    }
}
