namespace FinTrack.Api.Contracts.Goals
{
    public class FinancialGoalRequest
    {
        public int UserId { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
    }
}
