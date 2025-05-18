using CSharpFunctionalExtensions;
using FinTrack.Api.Data;
using FinTrack.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FinTrack.Api.Repository.Implementations
{
    public class BudgetRepository : DbRepository<Budget>
    {
        public override IQueryable<Budget> Items => 
            base.Items
            .Include(x => x.Currency)
            .Include(x => x.User)
            ;
        public BudgetRepository(FinTrackDbContext context) : base(context)
        {
        }

        public override Task<IReadOnlyList<Budget>> GetListAsync(object obj)
        {
            throw new NotImplementedException();
        }
    }



}
