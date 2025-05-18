using FinTrack.Api.Models.Base;

namespace FinTrack.Models;

public class Currency : NamedEntity
{
    public char Symbol { get; set; }
    public ICollection<Budget>? Budgets { get; set; }
}
