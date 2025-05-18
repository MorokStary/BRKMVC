using CSharpFunctionalExtensions;
using FinTrack.Models;

namespace FinTrack.Api.Services.Interfaces
{
    public interface IIncomeService
    {
        Task<Result<IReadOnlyList<Income>>> GetIncomesAsync(int budgetId);

        Task<Result> CreateIncomeAsync(Income income);
        Task<Result<Income>> GetIncomeAsync(int incomeId);
    }
}
