namespace FinTrack.Api.Contracts.Analysis
{
    public class AnalysisOverviewResponse
    {
        public decimal TotalExpenses { get; set; }
        public decimal MonthlyAverage { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public List<TopCategoryItem> TopCategories { get; set; }
    }

    public class TopCategoryItem
    {
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
