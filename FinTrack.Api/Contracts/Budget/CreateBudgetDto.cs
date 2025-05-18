namespace FinTrack.Api.Contracts.Budget
{
    public class CreateBudgetDto
    {
        public string Name { get; set; }
        public decimal PlannedAmountOfMoney { get; set; }
        public decimal TotalAmountOfMoney { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
