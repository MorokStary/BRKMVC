using CSharpFunctionalExtensions;
using FinTrack.Api.Repository.Interfaces;
using FinTrack.Api.Services.Interfaces;
using FinTrack.Models;

namespace FinTrack.Api.Services.Implementations
{
    public class IncomeCategoryService : IIncomeCategoryService
    {
        private readonly IRepository<IncomeCategory> _incomeCategoryRepository;

        public IncomeCategoryService(IRepository<IncomeCategory> incomeCategoryRepository)
        {
            this._incomeCategoryRepository = incomeCategoryRepository;
        }
        public async Task<Result> CreateIncomeCategoryAsync(IncomeCategory incomeCategoryCreate)
        {
            if (incomeCategoryCreate == null)
            {
                return Result.Failure("Null reference object");
            }

            var incomeCategory = _incomeCategoryRepository
               .Items
               .FirstOrDefault(x =>
               x.Name.Trim().ToUpper() == incomeCategoryCreate.Name.Trim().ToUpper());

            if (incomeCategory != null)
            {
                return Result.Failure("A category with the same name is already exists");
            }

            if (!(await _incomeCategoryRepository.CreateAsync(incomeCategoryCreate)))
            {
                return Result.Failure("Something went wrong while saving");
            }
            return Result.Success();

        }

        public async Task<Result<IReadOnlyList<IncomeCategory>>> GetIncomeCategoriesAsync(int userId)
        {

            var incomeCategoriesByUser = await _incomeCategoryRepository.GetListAsync(userId);

            var combinedCategories = (await _incomeCategoryRepository.GetListAsync())
                  .Where(x => x.User == null)
                  .Concat(incomeCategoriesByUser)
                  .ToList()
                  .AsReadOnly();

            return Result.Success<IReadOnlyList<IncomeCategory>>(combinedCategories);
        }


    }
}
