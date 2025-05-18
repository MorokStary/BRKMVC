using FinTrack.Api.Models.Base;

namespace FinTrack.Models;
public class Income : NamedEntity
{
    public string Description { get; set; }
    public decimal IncomeVolume { get; set; }
    public DateTime IncomeDate { get; set; }
    public int? IncomeCategoryId { get; set; }
    public IncomeCategory? IncomeCategory { get; set; }
    public int BudgetId { get; set; }
    public Budget Budget{ get; set; }

}
