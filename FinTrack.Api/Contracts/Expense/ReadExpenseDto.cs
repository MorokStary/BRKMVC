using FinTrack.Api.Contracts.ExpenseCategory;

namespace FinTrack.Api.Contracts.Expense
{
    public class ReadExpenseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal ExpenseVolume { get; set; }
        public ReadExpenseCategoryDto ExpenseCategory { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
