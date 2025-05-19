using FinTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Api.Data
{
    public static class BigSeedData
    {
        public static async Task EnsureSeededAsync(FinTrackDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.Users.AnyAsync(u => u.Name == "BigTest User")) return;

            var user = new User
            {
                Name = "BigTest User",
                Password = "test123"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var currency = new Currency
            {
                Name = "Test ₴",
                Symbol = '₴'
            };
            context.Currencies.Add(currency);
            await context.SaveChangesAsync();

            var budget = new Budget
            {
                Name = "Big Test Budget",
                PlannedAmountOfMoney = 10000,
                TotalAmountOfMoney = 5000,
                CurrencyId = currency.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow.AddMonths(-3)
            };
            context.Budgets.Add(budget);
            await context.SaveChangesAsync();

            var categories = new List<ExpenseCategory>();
            var categoryNames = new[] { "Food", "Entertainment", "Transport", "Shopping", "Services" };
            foreach (var name in categoryNames)
            {
                var category = new ExpenseCategory
                {
                    Name = name,
                    UserId = user.Id
                };
                categories.Add(category);
                context.ExpenseCategories.Add(category);
            }
            await context.SaveChangesAsync();

            var expenses = new List<Expense>();
            var startDate = DateTime.UtcNow.AddMonths(-3);
            var rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                var date = startDate.AddDays(i);
                var category = categories[i % categories.Count];
                var amount = rand.Next(50, 300); // between 50 and 300

                expenses.Add(new Expense
                {
                    Name = $"Seeded-Expense-{i + 1}",
                    Description = $"Seeded Expense {i + 1}",
                    ExpenseVolume = amount,
                    ExpenseDate = date,
                    ExpenseCategoryId = category.Id,
                    BudgetId = budget.Id,
                    Budget = budget
                });
            }

            context.Expenses.AddRange(expenses);

            var incomeCategory = new IncomeCategory
            {
                Name = "Salary",
                UserId = user.Id
            };
            context.IncomeCategories.Add(incomeCategory);
            await context.SaveChangesAsync();

            var incomes = new List<Income>();
            for (int i = 0; i < 30; i++)
            {
                var date = startDate.AddDays(i * 3);
                var amount = rand.Next(500, 2000);

                incomes.Add(new Income
                {
                    Name = $"Seeded-Income-{i + 1}",
                    Description = $"Income {i + 1}",
                    IncomeVolume = amount,
                    IncomeDate = date,
                    BudgetId = budget.Id,
                    Budget = budget,
                    IncomeCategoryId = incomeCategory.Id
                });
            }

            context.Incomes.AddRange(incomes);
            await context.SaveChangesAsync();
        }
    }
}
