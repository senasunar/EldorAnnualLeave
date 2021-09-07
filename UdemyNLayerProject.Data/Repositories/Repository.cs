using EldorAnnualLeave.Core.Repositories;
using EldorAnnualLeave.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly ApplicationIdentityDbContext _identityContext;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly DbSet<TEntity> _dbSetIdentity;

        public Repository(AppDbContext context, ApplicationIdentityDbContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
            _dbSet = context.Set<TEntity>();
            _dbSetIdentity = identityContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync([Optional]int id, [Optional] string sid)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate);
        }

        public TEntity Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public async Task AddUserAsync(TEntity entity)
        {
            await _dbSetIdentity.AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllUsersAsync()
        {
            return await _dbSetIdentity.ToListAsync();
        }

        public async Task<TEntity> GetByUserIdAsync(String id)
        {
            return await _dbSetIdentity.FindAsync(id);
        }

        public void RemoveUser(TEntity entity)
        {
            _dbSetIdentity.Remove(entity);
        }

        public TEntity UpdateUser(TEntity entity)
        {
            _identityContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}