using CSharpFunctionalExtensions;
using FinTrack.Models;

namespace FinTrack.Api.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<Result<IReadOnlyList<Currency>>> GetCurrenciess();

        Task<Result> CreateCurrency(Currency currency);
        Task<Result> IsCurrencyExists(int currencyId);
    }
}
