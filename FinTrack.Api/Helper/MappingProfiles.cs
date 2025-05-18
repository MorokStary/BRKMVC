using AutoMapper;
using FinTrack.Api.Contracts.Budget;
using FinTrack.Api.Contracts.Expense;
using FinTrack.Api.Contracts.ExpenseCategory;
using FinTrack.Api.Contracts.Income;
using FinTrack.Api.Contracts.IncomeCategory;
using FinTrack.Api.Contracts.User;
using FinTrack.Api.Contracts.Сurrency;
using FinTrack.Models;
namespace FinTrack.Api.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Currency, ReadCurrencyDto>();
            CreateMap<CreateCurrencyDto, Currency>();

            CreateMap<Budget, ReadBudgetDto>();
            CreateMap<CreateBudgetDto, Budget>();

            CreateMap<ExpenseCategory, ReadExpenseCategoryDto>();
            CreateMap<CreateExpenseCategoryDto, ExpenseCategory>();

            CreateMap<IncomeCategory, ReadIncomeCategoryDto>();
            CreateMap<CreateIncomeCategoryDto, IncomeCategory>();

            CreateMap<Expense, ReadExpenseDto>();
            CreateMap<CreateExpenseDto, Expense>();

            CreateMap<Income, ReadIncomeDto>();
            CreateMap<CreateIncomeDto, Income>();

            CreateMap<User, ReadUserDto>();
            CreateMap<CreateUserDto, User>();



        }
    }
}
