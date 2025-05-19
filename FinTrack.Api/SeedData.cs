using FinTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Api.Data
{
    public static class SeedData
    {
        public static async Task EnsureSeededAsync(FinTrackDbContext context)
        {
            await context.Database.MigrateAsync();

            // 1. Користувач
            var user = await context.Users.FirstOrDefaultAsync(u => u.Name == "Test User");
            if (user == null)
            {
                user = new User
                {
                    Name = "Test User",
                    Password = "test123"
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            // 2. Валюта
            var currency = await context.Currencies.FirstOrDefaultAsync(c => c.Name == "Test Dollar");
            if (currency == null)
            {
                currency = new Currency
                {
                    Name = "Test Dollar",
                    Symbol = '$'
                };
                context.Currencies.Add(currency);
                await context.SaveChangesAsync();
            }

            // 3. Бюджет
            var budget = await context.Budgets.FirstOrDefaultAsync(b => b.Name == "Test Budget" && b.UserId == user.Id);
            if (budget == null)
            {
                budget = new Budget
                {
                    Name = "Test Budget",
                    PlannedAmountOfMoney = 5000,
                    TotalAmountOfMoney = 2000,
                    CreationDate = DateTime.UtcNow.AddMonths(-2),
                    CurrencyId = currency.Id,
                    UserId = user.Id
                };
                context.Budgets.Add(budget);
                await context.SaveChangesAsync();
            }

            // 4. Категорія
            var category = await context.ExpenseCategories.FirstOrDefaultAsync(c => c.Name == "Food" && c.UserId == user.Id);
            if (category == null)
            {
                category = new ExpenseCategory
                {
                    Name = "Food",
                    UserId = user.Id
                };
                context.ExpenseCategories.Add(category);
                await context.SaveChangesAsync();
            }

            // 5. Витрати
            var existingExpenses = await context.Expenses
                .Where(e => e.BudgetId == budget.Id && e.Description.StartsWith("Seeded-"))
                .ToListAsync();

            if (!existingExpenses.Any())
            {
                context.Expenses.AddRange(
                new Expense
                {
                    Name = "Seeded-Lunch",
                    Description = "Seeded-Lunch",
                    ExpenseVolume = 20,
                    ExpenseDate = DateTime.UtcNow.AddDays(-40),
                    BudgetId = budget.Id,
                    Budget = budget,
                    ExpenseCategoryId = category.Id
                },
                new Expense
                {
                    Name = "Seeded-Dinner",
                    Description = "Seeded-Dinner",
                    ExpenseVolume = 25,
                    ExpenseDate = DateTime.UtcNow.AddDays(-10),
                    BudgetId = budget.Id,
                    Budget = budget,
                    ExpenseCategoryId = category.Id
                },
                new Expense
                {
                    Name = "Seeded-Snacks",
                    Description = "Seeded-Snacks",
                    ExpenseVolume = 15,
                    ExpenseDate = DateTime.UtcNow.AddDays(-5),
                    BudgetId = budget.Id,
                    Budget = budget,
                    ExpenseCategoryId = category.Id
                }
                );


                await context.SaveChangesAsync();
            }
        }
    }
}
