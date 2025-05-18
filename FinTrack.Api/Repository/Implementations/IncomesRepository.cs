using FinTrack.Api.Data;
using FinTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Api.Repository.Implementations
{
    public class IncomesRepository : DbRepository<Income>
    {
        public override IQueryable<Income> Items =>
            base.Items
            .Include(x => x.IncomeCategory)
            ;
        public IncomesRepository(FinTrackDbContext context) : base(context)
        {
        }


        public override async Task<IReadOnlyList<Income>> GetListAsync(object obj)
        {
            if (!(obj is Budget budget)) return new List<Income>();

            return await Items.AsNoTracking().Where(x => x.BudgetId == budget.Id).ToListAsync();
        }
    }
}
