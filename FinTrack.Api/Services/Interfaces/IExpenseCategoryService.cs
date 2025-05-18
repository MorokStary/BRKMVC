using CSharpFunctionalExtensions;
using FinTrack.Models;

namespace FinTrack.Api.Services.Interfaces
{
    public interface IExpenseCategoryService
    {
        Task<Result<IReadOnlyList<ExpenseCategory>>> GetExpenseCategoriesAsync(int userId);
        Task<Result> CreateExpenseCategoryAsync(ExpenseCategory expenseCategory);
        Task<Result<ExpenseCategory>> GetExpenseCategory(int categoryId);
    }
}
