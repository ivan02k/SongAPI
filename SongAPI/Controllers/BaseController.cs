using Data.Entities.IdentityClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace SongAPI.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController<T> : ControllerBase
    {
        IBaseService<T> _service;
        public BaseController(IBaseService<T> service)
        {
            _service = service;
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
            return _service.GetAll();
        }

        [HttpGet("byName")]
        [AllowAnonymous]
        public string GetBy(string byName)
        {
            return _service.GetByName(byName);
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
