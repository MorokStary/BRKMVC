using FinTrack.Api.Models.Base;

namespace FinTrack.Models;

public class ExpenseCategory : NamedEntity
{ 
    public int? UserId { get; set; }
    public User? User { get; set; }
    public ICollection<Expense>? Expenses{ get; set; }
}
