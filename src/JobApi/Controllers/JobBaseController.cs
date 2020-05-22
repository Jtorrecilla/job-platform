using Db.Core;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobApi.Controllers
{
    [ApiController]
    public class JobBaseController<TViewModel, TEntity, TType> : ControllerBase
        where TViewModel : BaseViewModel<TType>
        where TEntity : BaseEntity<TType>
    {
        private IBaseService<TViewModel, TEntity, TType> _service;

        public JobBaseController(IBaseService<TViewModel, TEntity, TType> service)
        {
            _service = service;
        }
        [HttpGet, Route("{id}")]
        public async Task<TViewModel> Get(TType id)
        {
            return await _service.Get(id);
        }
        [HttpGet, Route("all")]
        public async Task<List<TViewModel>> Get()
        {
            return await _service.Get();
        }
        [HttpPost]
        [Consumes("application/json")]
        public async Task<TViewModel> Post([FromBody]TViewModel model)
        {
            return await _service.Add(model);
        }
        [HttpPut]
        [Consumes("application/json")]
        public async Task<TViewModel> Put([FromBody]TViewModel model)
        {
            return await _service.Update(model);
        }
        [HttpDelete, Route("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> Delete(TType id)
        {
            await _service.Delete(id);
            return NoContent();
        }
      
    }
}
