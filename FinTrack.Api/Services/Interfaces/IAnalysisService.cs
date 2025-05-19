using FinTrack.Api.Contracts.Analysis;

namespace FinTrack.Api.Interfaces
{
    public interface IAnalysisService
    {
        Task<AnalysisOverviewResponse> GetOverviewAsync(int userId, DateTime from, DateTime to);
        Task<List<RecommendationResponse>> GetRecommendationsAsync(int userId, DateTime from, DateTime to);
        Task<List<AnomalyResponse>> GetAnomaliesAsync(int userId, DateTime from, DateTime to);
    }
}
