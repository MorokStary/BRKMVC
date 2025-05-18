namespace FinTrack.Api.Contracts.Income
{
    public class CreateIncomeDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal IncomeVolume { get; set; }
        public int IncomeCategoryId { get; set; }
        public DateTime IncomeDate { get; set; }
    }
}
