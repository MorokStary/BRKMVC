using CSharpFunctionalExtensions;
using FinTrack.Api.Repository.Implementations;
using FinTrack.Api.Repository.Interfaces;
using FinTrack.Api.Services.Interfaces;
using FinTrack.Models;

namespace FinTrack.Api.Services.Implementations
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IRepository<ExpenseCategory> _expenseCategoryRepository;

        public ExpenseCategoryService(IRepository<ExpenseCategory> expenseCategoryRepository)
        {
            this._expenseCategoryRepository = expenseCategoryRepository;
        }
        public async Task<Result> CreateExpenseCategoryAsync(ExpenseCategory expenseCategoryCreate)
        {
            if(expenseCategoryCreate == null) 
            {
                return Result.Failure("Null reference object");
            }

            var expenseCategory = _expenseCategoryRepository
               .Items
               .FirstOrDefault(
               x => x.UserId == expenseCategoryCreate.UserId &&
               x.Name.Trim().ToUpper() == expenseCategoryCreate.Name.Trim().ToUpper());

            if (expenseCategory != null)
            {
                return Result.Failure("A category with the same name is already exists");
            }

            if (!(await _expenseCategoryRepository.CreateAsync(expenseCategoryCreate)))
            {
                return Result.Failure("Something went wrong while saving");
            }
            return Result.Success();

        }

        public async Task<Result<IReadOnlyList<ExpenseCategory>>> GetExpenseCategoriesAsync(int userId)
        {
            
            var expenseCategoriesByUser = await _expenseCategoryRepository.GetListAsync(new User { Name = " ", Id = userId});

            var combinedCategories =  (await _expenseCategoryRepository.GetListAsync())
                  .Where(x => x.UserId == null)
                  .Concat(expenseCategoriesByUser)
                  .ToList()
                  .AsReadOnly();

            return Result.Success<IReadOnlyList<ExpenseCategory>>(combinedCategories);
        }

        public async Task<Result<ExpenseCategory>> GetExpenseCategory(int categoryId)
        {
            return await _expenseCategoryRepository.GetItemAsync(categoryId);
        }


    }
}
