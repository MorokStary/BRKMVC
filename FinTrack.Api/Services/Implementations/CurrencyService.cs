using CSharpFunctionalExtensions;
using FinTrack.Api.Repository.Interfaces;
using FinTrack.Api.Services.Interfaces;
using FinTrack.Models;

namespace FinTrack.Api.Services.Implementations
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IRepository<Currency> _currencyRepository;

        public CurrencyService(IRepository<Currency> currencyRepository)
        {
            this._currencyRepository = currencyRepository;
        }

        public async Task<Result> CreateCurrency(Currency currencyCreate)
        {
            if (currencyCreate == null)
                return Result.Failure("Object does not exists");

            if( await _currencyRepository.IsItemExistsAsync(currencyCreate.Name))
                return Result.Failure("This currency is already exists");

            if(!await _currencyRepository.CreateAsync(currencyCreate))
                return Result.Failure("Something went wrong while saving");

            return Result.Success();
        }

        public async Task<Result<IReadOnlyList<Currency>>> GetCurrenciess()
        {
            var currencies = await _currencyRepository.GetListAsync();

            if (currencies.Count == 0)
                return Result.Failure<IReadOnlyList<Currency>>("There are no currencies yet");

            return Result.Success(currencies);
        }
        public async Task<Result> IsCurrencyExists(int currencyId) =>
            await _currencyRepository.IsItemExistsAsync(currencyId)
                ? Result.Success()
                : Result.Failure("Non-exists identifier");
    }
}
