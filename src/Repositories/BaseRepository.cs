using Db.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories
{
    public class BaseRepository<TEntity, TType> : IRepository<TEntity, TType> where TEntity : BaseEntity<TType>
    {
        protected DbContext _context;
        protected DbSet<TEntity> _set;

        public BaseRepository(JobContext context)
        {
            _context = context;
            _set = context.Set<TEntity>();

        }
        public virtual Task<TEntity> Get(TType id)
        {
            return _set.FirstOrDefaultAsync(p => p.Id.Equals(id));
        }
        public Task<List<TEntity>> Get()
        {
            return _set.ToListAsync();
        }
        public virtual Task<List<TEntity>> GetByAgent(string agent, bool isAdmin)
        {
            return _set.ToListAsync();
        }
        public Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> where)
        {
            return _set.Where(where).ToListAsync();
        }
        public virtual Task<TEntity> Add(TEntity entity)
        {
            _set.Add(entity);
            _context.SaveChanges();
            return Task.FromResult<TEntity>(entity);
        }
        public virtual Task<TEntity> Update(TEntity entity)
        {

            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return Task.FromResult<TEntity>(entity);
        }
        public Task Delete(TType id)
        {
            var entity = _set.Find(id);
            _set.Remove(entity);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
