using Db.Core;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public abstract class BaseService<TViewModel, TEntity, TType> : IBaseService<TViewModel, TEntity, TType>
       where TViewModel : BaseViewModel<TType>
       where TEntity : BaseEntity<TType>
    {
        private IRepository<TEntity, TType> _repository;

        public BaseService(IRepository<TEntity, TType> repository)
        {
            _repository = repository;
        }

        public virtual async Task<TViewModel> Add(TViewModel viewModel)
        {
            var result = await _repository.Add(Map(viewModel));
            return Map(result);
        }

        public Task Delete(TType id)
        {

            return _repository.Delete(id);
        }

        public virtual async Task<TViewModel> Get(TType guid)
        {
            var result = await _repository.Get(guid);
            return Map(result);
        }
        public virtual async Task<List<TViewModel>> GetByAgent(string agent, bool isAdmin)
        {
            var result = await _repository.GetByAgent(agent, isAdmin);
            return result.Select(p => Map(p)).ToList();
        }
        public async Task<List<TViewModel>> Get()
        {
            var result = await _repository.Get();
            return result.Select(p => Map(p)).ToList();
        }

        public abstract TViewModel Map(TEntity entity);
        public abstract TEntity Map(TViewModel entity);

        public async Task<TViewModel> Update(TViewModel viewModel)
        {
            var result = await _repository.Update(Map(viewModel));
            return Map(result);
        }
    }
}
