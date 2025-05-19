using FinTrack.Api.Contracts.Goals;
using FinTrack.Api.Data;
using FinTrack.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Api.Services
{
    public class GoalAnalysisService : IGoalAnalysisService
    {
        private readonly FinTrackDbContext _dbContext;

        public GoalAnalysisService(FinTrackDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GoalRecommendationResponse> AnalyzeGoalAsync(FinancialGoalRequest request)
        {
            var now = DateTime.UtcNow;
            var monthsRemaining = (int)Math.Ceiling((request.TargetDate - now).TotalDays / 30.0);

            if (monthsRemaining <= 0)
                monthsRemaining = 1;

            // Поточний бюджет користувача
            var budgets = await _dbContext.Budgets
                .Where(b => b.UserId == request.UserId)
                .ToListAsync();

            var currentBalance = budgets.Sum(b => b.TotalAmountOfMoney);

            var amountNeeded = request.TargetAmount - currentBalance;
            if (amountNeeded <= 0)
            {
                return new GoalRecommendationResponse
                {
                    CanReachGoal = true,
                    CurrentBalance = currentBalance,
                    MonthlyTarget = 0,
                    AverageMonthlyFreeAmount = 0,
                    MonthsRemaining = monthsRemaining,
                    TargetDate = request.TargetDate,
                    Suggestions = new List<GoalCutSuggestion>()
                };
            }

            var monthlyTarget = amountNeeded / monthsRemaining;

            // Витрати і доходи за останні 3 місяці
            var dateThreshold = now.AddMonths(-3);

            var incomes = await _dbContext.Incomes
                .Where(i => i.Budget.UserId == request.UserId && i.IncomeDate >= dateThreshold)
                .ToListAsync();

            var expenses = await _dbContext.Expenses
                .Where(e => e.Budget.UserId == request.UserId && e.ExpenseDate >= dateThreshold && e.ExpenseCategory != null)
                .Include(e => e.ExpenseCategory)
                .ToListAsync();

            var totalIncome = incomes.Sum(i => i.IncomeVolume);
            var totalExpense = expenses.Sum(e => e.ExpenseVolume);

            var avgMonthlyFree = (totalIncome - totalExpense) / 3;

            var canReach = avgMonthlyFree >= monthlyTarget;

            var suggestions = new List<GoalCutSuggestion>();

            if (!canReach)
            {
                var grouped = expenses
                    .GroupBy(e => e.ExpenseCategory!.Name)
                    .Select(g => new
                    {
                        Category = g.Key,
                        Total = g.Sum(x => x.ExpenseVolume)
                    })
                    .OrderByDescending(g => g.Total)
                    .Take(5)
                    .ToList();

                decimal remainingGap = monthlyTarget - avgMonthlyFree;

                foreach (var group in grouped)
                {
                    if (remainingGap <= 0) break;

                    var cut = group.Total * 0.2m; // Пропонуємо скоротити на 20% кожну з топ категорій
                    cut = cut > remainingGap ? remainingGap : cut;

                    suggestions.Add(new GoalCutSuggestion
                    {
                        CategoryName = group.Category,
                        CutAmount = Math.Round(cut, 2),
                        Message = $"Спробуйте скоротити витрати в категорії \"{group.Category}\" на {Math.Round(cut, 2)}"
                    });

                    remainingGap -= cut;
                }
            }

            return new GoalRecommendationResponse
            {
                CanReachGoal = canReach,
                CurrentBalance = currentBalance,
                MonthlyTarget = Math.Round(monthlyTarget, 2),
                AverageMonthlyFreeAmount = Math.Round(avgMonthlyFree, 2),
                MonthsRemaining = monthsRemaining,
                TargetDate = request.TargetDate,
                Suggestions = suggestions
            };
        }
    }
}
