using Db.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBaseService<TViewModel, TEntity, TType>
     where TViewModel : BaseViewModel<TType>
     where TEntity : BaseEntity<TType>
    {
        Task<TViewModel> Get(TType guid);
        Task<List<TViewModel>> Get();
        Task<TViewModel> Add(TViewModel viewModel);
        Task<TViewModel> Update(TViewModel viewModel);
        Task Delete(TType id);
        TViewModel Map(TEntity entity);
    }
}
