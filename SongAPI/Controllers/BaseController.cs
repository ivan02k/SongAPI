using Data.Entities.IdentityClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace SongAPI.Controllers
{
    public class BaseController<T> : ControllerBase
    {
        IBaseService<T> _service;
        public BaseController(IBaseService<T> service)
        {
            _service = service;
        }

        [HttpPost]
        public bool Create(T obj)
        {
            return _service.Create(obj);
        }

        [HttpGet]
    
        public List<string> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("byName")]
        public string GetById(string byName)
        {
            return _service.GetByName(byName);
        }

        [HttpPut]
        public bool Update(T obj, string name)
        {
            return _service.Update(obj, name);
        }

        [HttpDelete("byName")]
        public bool Delete(string byName)
        {
            return _service.Delete(byName);
        }
    }
}
