using FinTrack.Api.Repository.Implementations;
using FinTrack.Api.Repository.Interfaces;
using FinTrack.Models;

namespace FinTrack.Api.Repository
{
    public static class RepositoryRegistrator
    {
        public static IServiceCollection AddRepositoriesInDb(this IServiceCollection services) => services
            .AddTransient<IRepository<Currency>, DbRepository<Currency>>()
            .AddTransient<IRepository<Expense>, ExpensesRepository>()
            .AddTransient<IRepository<Budget>, BudgetRepository>()
            .AddTransient<IRepository<ExpenseCategory>, ExpenseCategoriesRepository>()
            .AddTransient<IRepository<Income>, IncomesRepository>()
            .AddTransient<IRepository<IncomeCategory>, IncomeCategoriesRepository>()
            .AddTransient<IRepository<User>, DbRepository<User>>();
    }
}
