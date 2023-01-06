using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using ViewModels;

namespace SongAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Login")]
        public Task LogIn(LogInViewModel obj)
        {
            return _userService.LogIn(obj);
        }
        [HttpPost("Register")]
        public Task Register(RegistrationViewModel obj)
        {
           return _userService.Register(obj);
        }
    }
}
