using FinTrack.Api.Data;
using FinTrack.Api.Models.Base;
using FinTrack.Api.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace FinTrack.Api.Repository.Implementations
{
    public class DbRepository<T> : IRepository<T> where T : NamedEntity
    {
        protected readonly FinTrackDbContext _context;
        private readonly DbSet<T> _set;
        
        public virtual IQueryable<T> Items => _set;
        public DbRepository(FinTrackDbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }
        public async Task<bool> CreateAsync(T obj)
        {
            await _context.AddAsync(obj);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(T obj)
        {
            _context.Remove(obj);
            return await SaveAsync();
        }

        public async Task<T> GetItemAsync(int id)
        {
            return await Items.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> GetListAsync()
        {
            return await Items.AsNoTracking().ToListAsync();
        }


        public virtual Task<IReadOnlyList<T>> GetListAsync(object obj)
        {
            return null;
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateAsync(T obj)
        {
            _context.Update(obj);
            return await SaveAsync();
        }

        public async Task<bool> IsItemExistsAsync(string name)
        {
            return await Items.AnyAsync(x => x.Name == name);
        }
        public async Task<bool> IsItemExistsAsync(int id)
        {
            return await Items.AnyAsync(x => x.Id == id);
        }

        public async Task<T> GetItemByNameAsync(string name)
        {
            return await Items.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
