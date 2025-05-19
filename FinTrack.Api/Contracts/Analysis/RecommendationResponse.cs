namespace FinTrack.Api.Contracts.Analysis
{
    public class AnomalyResponse
    {
        public string CategoryName { get; set; }
        public decimal PreviousPeriodAmount { get; set; }
        public decimal CurrentPeriodAmount { get; set; }
        public double DifferencePercent { get; set; }
    }
}
