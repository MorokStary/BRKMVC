using CSharpFunctionalExtensions;
using FinTrack.Api.Repository.Interfaces;
using FinTrack.Api.Services.Interfaces;
using FinTrack.Models;

namespace FinTrack.Api.Services.Implementations
{
    public class IncomeService : IIncomeService
    {
        private readonly IRepository<Income> _incomesRepository;
        private readonly IRepository<Budget> _budgetRepository;

        public IncomeService(IRepository<Income> incomesRepository, IRepository<Budget> budgetRepository)
        {
           _incomesRepository = incomesRepository;
           _budgetRepository = budgetRepository;
        }
        public async Task<Result> CreateIncomeAsync(Income incomeCreate)
        {
            if (incomeCreate == null)
            {
                return Result.Failure("Null reference object");
            }

            if (!await _incomesRepository.CreateAsync(incomeCreate))
                return Result.Failure("Something went wrong while saving");

            var budgetUpdate = await _budgetRepository.GetItemAsync(incomeCreate.BudgetId);
            budgetUpdate.TotalAmountOfMoney += incomeCreate.IncomeVolume;

            await _budgetRepository.UpdateAsync(budgetUpdate);

            return Result.Success();
        }

        public async Task<Result<IReadOnlyList<Income>>> GetIncomesAsync(int budgetId)
        {
            if (!await _budgetRepository.IsItemExistsAsync(budgetId))
                return Result.Failure<IReadOnlyList<Income>>("Incorrect budget Id");

            var incomes = await _incomesRepository.GetListAsync(new Budget() { Id = budgetId, Name = "" });

            if (incomes == null)
                return Result.Failure<IReadOnlyList<Income>>("There are no incomes in this budget");

            return Result.Success(incomes);
        }

        public async Task<Result<Income>> GetIncomeAsync(int incomeId)
        {
            if (!await _incomesRepository.IsItemExistsAsync(incomeId))
                return Result.Failure<Income>("Incorrect income id");
            var income = await _incomesRepository.GetItemAsync(incomeId);
            if (income == null)
                return Result.Failure<Income>("There is no income for this identifier");
            return Result.Success(income);

        }
    }
}
