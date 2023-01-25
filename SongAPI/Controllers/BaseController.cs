using Data.Entities.IdentityClass;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using ViewModels.Data.ViewModels;

namespace SongAPI.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController<T> : ControllerBase
    {
        IBaseService<T> _service;
        ICached<T> _cached;

        public BaseController(IBaseService<T> service, ICached<T> cached)
        {
            _service = service;
            _cached = cached;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
            Roles = "Manager,Admin" )]
        public bool Create(T obj)
        {
            return _service.Create(obj);
        }

        [HttpGet]
        [AllowAnonymous]
        public List<string> GetAll()
        {
            return _cached.GetAll();
        }

        [HttpGet("byName")]
        [AllowAnonymous]
        public string GetBy(string byName)
        {
            return _cached.GetByName(byName);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public bool Update(T obj, string name)
        {
            return _service.Update(obj, name);
        }

        [HttpDelete("byName")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public bool Delete(string byName)
        {
            return _service.Delete(byName);
        }
    }
}
