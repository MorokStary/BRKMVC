using FinTrack.Api.Contracts.Goals;

namespace FinTrack.Api.Interfaces
{
    public interface IGoalAnalysisService
    {
        Task<GoalRecommendationResponse> AnalyzeGoalAsync(FinancialGoalRequest request);
    }
}
