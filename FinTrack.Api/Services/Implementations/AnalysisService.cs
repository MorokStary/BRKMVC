using FinTrack.Api.Contracts.Analysis;
using FinTrack.Api.Data;
using FinTrack.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Api.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly FinTrackDbContext _dbContext;

        public AnalysisService(FinTrackDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AnalysisOverviewResponse> GetOverviewAsync(int userId, DateTime from, DateTime to)
        {
            var expenses = await _dbContext.Expenses
                .Include(e => e.ExpenseCategory)
                .Include(e => e.Budget)
                .Where(e => e.Budget.UserId == userId && e.ExpenseDate >= from && e.ExpenseDate <= to)
                .ToListAsync();

            var total = expenses.Sum(e => e.ExpenseVolume);

            var periodDays = (to - from).TotalDays;
            var monthlyAvg = periodDays > 0 ? total / (decimal)(periodDays / 30) : 0;

            var topCategories = expenses
                .Where(e => e.ExpenseCategory != null)
                .GroupBy(e => e.ExpenseCategory!.Name)
                .Select(g => new TopCategoryItem
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(x => x.ExpenseVolume)
                })
                .OrderByDescending(c => c.TotalAmount)
                .Take(5)
                .ToList();

            return new AnalysisOverviewResponse
            {
                TotalExpenses = total,
                MonthlyAverage = Math.Round(monthlyAvg, 2),
                PeriodStart = from,
                PeriodEnd = to,
                TopCategories = topCategories
            };
        }

        public async Task<List<RecommendationResponse>> GetRecommendationsAsync(int userId, DateTime from, DateTime to)
        {
            var expenses = await _dbContext.Expenses
                .Include(e => e.ExpenseCategory)
                .Include(e => e.Budget)
                .Where(e => e.Budget.UserId == userId && e.ExpenseDate >= from && e.ExpenseDate <= to)
                .ToListAsync();

            var recommendations = new List<RecommendationResponse>();

            var grouped = expenses
                .Where(e => e.ExpenseCategory != null)
                .GroupBy(e => e.ExpenseCategory!.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(x => x.ExpenseVolume),
                    Count = g.Count()
                })
                .ToList();

            foreach (var item in grouped)
            {
                if (item.Total > 1000)
                {
                    recommendations.Add(new RecommendationResponse
                    {
                        CategoryName = item.Category,
                        Message = $"Ваші витрати в категорії \"{item.Category}\" перевищують 1000 за вибраний період.",
                        Severity = RecommendationSeverity.High,
                        SuggestedLimit = item.Total * 0.8m
                    });
                }
                else if (item.Count > 10)
                {
                    recommendations.Add(new RecommendationResponse
                    {
                        CategoryName = item.Category,
                        Message = $"У категорії \"{item.Category}\" зафіксовано понад 10 транзакцій. Можливо, варто об’єднати їх або проаналізувати регулярність.",
                        Severity = RecommendationSeverity.Medium,
                        SuggestedLimit = null
                    });
                }
            }

            if (!recommendations.Any())
            {
                recommendations.Add(new RecommendationResponse
                {
                    CategoryName = null,
                    Message = "Витрати в межах помірного рівня. Немає критичних рекомендацій.",
                    Severity = RecommendationSeverity.Low,
                    SuggestedLimit = null
                });
            }

            return recommendations;
        }

        public async Task<List<AnomalyResponse>> GetAnomaliesAsync(int userId, DateTime from, DateTime to)
        {
            var previousFrom = from.AddDays(-(to - from).TotalDays);
            var previousTo = from;

            var expenses = await _dbContext.Expenses
                .Include(e => e.Budget)
                .Include(e => e.ExpenseCategory)
                .Where(e => e.Budget.UserId == userId && e.ExpenseCategory != null)
                .ToListAsync();

            var previous = expenses
                .Where(e => e.ExpenseDate >= previousFrom && e.ExpenseDate < previousTo)
                .GroupBy(e => e.ExpenseCategory!.Name)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.ExpenseVolume));

            var current = expenses
                .Where(e => e.ExpenseDate >= from && e.ExpenseDate <= to)
                .GroupBy(e => e.ExpenseCategory!.Name)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.ExpenseVolume));

            var allCategories = previous.Keys.Union(current.Keys).Distinct();

            var result = new List<AnomalyResponse>();

            foreach (var category in allCategories)
            {
                var prevAmount = previous.ContainsKey(category) ? previous[category] : 0;
                var currAmount = current.ContainsKey(category) ? current[category] : 0;

                if (prevAmount == 0 && currAmount == 0)
                    continue;

                double diffPercent;

                if (prevAmount == 0)
                {
                    diffPercent = 100;
                }
                else
                {
                    diffPercent = (double)((currAmount - prevAmount) / prevAmount) * 100;
                }

                if (Math.Abs(diffPercent) >= 50)
                {
                    result.Add(new AnomalyResponse
                    {
                        CategoryName = category,
                        PreviousPeriodAmount = prevAmount,
                        CurrentPeriodAmount = currAmount,
                        DifferencePercent = Math.Round(diffPercent, 2)
                    });
                }
            }

            return result;
        }

    }
}
