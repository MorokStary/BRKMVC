using FinTrack.Api.Interfaces;
using FinTrack.Api.Services.Implementations;
using FinTrack.Api.Services.Interfaces;

namespace FinTrack.Api.Services
{
    public static  class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddTransient<IBudgetService, BudgetService>()
            .AddTransient<ICurrencyService, CurrencyService>()
            .AddTransient<IExpenseCategoryService, ExpenseCategoryService>()
            .AddTransient<IIncomeCategoryService, IncomeCategoryService>()
            .AddTransient<IExpenseService, ExpenseService>()
            .AddTransient<IIncomeService, IncomeService>()
            .AddTransient<IUserService, UserService>()
            .AddTransient<IAnalysisService, AnalysisService>()
            ;
    }
}
