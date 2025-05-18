using CSharpFunctionalExtensions;
using FinTrack.Api.Repository.Implementations;
using FinTrack.Api.Repository.Interfaces;
using FinTrack.Api.Services.Interfaces;
using FinTrack.Models;

namespace FinTrack.Api.Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly IRepository<Expense> _expenseRepository;
        private readonly IRepository<Budget> _budgetRepository;

        public ExpenseService(IRepository<Expense> expensesRepository, IRepository<Budget> budgetRepository)
        {
            this._expenseRepository = expensesRepository;
            this._budgetRepository = budgetRepository;
        }
        public async Task<Result> CreateExpenseAsync(Expense expenseCreate)
        {
            if(expenseCreate == null) 
            { 
                return Result.Failure("Null reference object"); 
            }
            //expenseCreate.ExpenceCategoryId; 
            if(!await _expenseRepository.CreateAsync(expenseCreate))
                return Result.Failure("Something went wrong while saving");



            var budgetUpdate = await _budgetRepository.GetItemAsync(expenseCreate.BudgetId);
            budgetUpdate.TotalAmountOfMoney -= expenseCreate.ExpenseVolume;

            await _budgetRepository.UpdateAsync(budgetUpdate);

            return Result.Success();
        }

        public async Task<Result<IReadOnlyList<Expense>>> GetExpensesAsync(int budgetId)
        {
            if (!await _budgetRepository.IsItemExistsAsync(budgetId))
                return Result.Failure<IReadOnlyList<Expense>>("Incorrect budget Id");

            var expenses = await _expenseRepository.GetListAsync(new Budget() { Id = budgetId, Name = "" });

            if (expenses == null)
                return Result.Failure<IReadOnlyList<Expense>>("There are no expenses in this budget");

            return Result.Success(expenses);
        }

        public async Task<Result<Expense>> GetExpenseAsync(int expenseId)
        {
            if (!await _expenseRepository.IsItemExistsAsync(expenseId))
                return Result.Failure<Expense>("Incorrect expense id");
            var expense = await _expenseRepository.GetItemAsync(expenseId);
            if (expense == null)
                return Result.Failure<Expense>("There is no expense for this identifier");
            return Result.Success(expense);

        }
    }
}
