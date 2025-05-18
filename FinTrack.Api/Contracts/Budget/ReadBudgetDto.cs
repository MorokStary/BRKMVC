using FinTrack.Models;

namespace FinTrack.Api.Contracts.Budget
{
    public class ReadBudgetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PlannedAmountOfMoney { get; set; }

        public decimal TotalAmountOfMoney { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
