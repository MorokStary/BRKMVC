using CSharpFunctionalExtensions;
using FinTrack.Models;

namespace FinTrack.Api.Services.Interfaces
{
    public interface IIncomeCategoryService
    {
        Task<Result<IReadOnlyList<IncomeCategory>>> GetIncomeCategoriesAsync(int userId);

        Task<Result> CreateIncomeCategoryAsync(IncomeCategory incomeCategory);

    }
}
