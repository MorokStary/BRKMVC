using FinTrack.Api.Data;
using FinTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Api.Repository.Implementations
{
    public class IncomeCategoriesRepository : DbRepository<IncomeCategory>
    {
        public override IQueryable<IncomeCategory> Items =>
            base.Items
            .Include(x => x.Incomes)
            ;
        public IncomeCategoriesRepository(FinTrackDbContext context) : base(context)
        {
        }

        public override async Task<IReadOnlyList<IncomeCategory>> GetListAsync(object obj)
        {
            if (!(obj is User user)) return new List<IncomeCategory>();
            return null;
            //return await Items.AsNoTracking().Where(x => x.User.Id == user.Id).ToListAsync();
        }
    }
}
