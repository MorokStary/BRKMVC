namespace FinTrack.Api.Contracts.Goals
{
    public class GoalRecommendationResponse
    {
        public bool CanReachGoal { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal MonthlyTarget { get; set; }
        public decimal AverageMonthlyFreeAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public int MonthsRemaining { get; set; }
        public List<GoalCutSuggestion>? Suggestions { get; set; }
    }

    public class GoalCutSuggestion
    {
        public string CategoryName { get; set; }
        public decimal CutAmount { get; set; }
        public string Message { get; set; }
    }
}
