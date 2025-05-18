namespace FinTrack.Api.Contracts.Expense
{
    public class CreateExpenseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal ExpenseVolume { get; set; }
        public int ExpenseCategoryId { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
