using FinTrack.Api.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinTrack.Models;

public class IncomeCategory : NamedEntity
{
    public int? UserId { get; set; }
    public User? User { get; set; }
    public ICollection<Income>? Incomes { get; set; }
}
