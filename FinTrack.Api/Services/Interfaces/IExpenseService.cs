using CSharpFunctionalExtensions;
using FinTrack.Models;

namespace FinTrack.Api.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<Result<IReadOnlyList<Expense>>> GetExpensesAsync(int budgetId);
        Task<Result> CreateExpenseAsync(Expense expense);
        Task<Result<Expense>> GetExpenseAsync(int expenseId);
    }
}
