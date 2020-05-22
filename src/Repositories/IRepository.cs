using Db.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRepository<TEntity, TType> where TEntity : BaseEntity<TType>
    {
        Task<TEntity> Add(TEntity entity);
        Task Delete(TType entity);
        Task<List<TEntity>> Get();
        Task<List<TEntity>> GetByAgent(string agent, bool isAdmin);
        Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> where);
        Task<TEntity> Get(TType id);
        Task<TEntity> Update(TEntity entity);
    }

}
