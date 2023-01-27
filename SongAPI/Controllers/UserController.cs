using AutoMapper;
using Data.Entities.IdentityClass;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Interfaces;
using ViewModels;

namespace SongAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var user = _userService.Authenticate(model);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            AuthenticateResponse response = _userService.GetToken(user);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            if (model.FirstName == null
                && model.LastName == null
                && model.Username == null
                && model.Password == null)
            {
                return Ok(new { message = "Incorrect" });
            }
            _userService.Register(model);
            return Ok(new { message = "Registration successful" });
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
         Roles = "Admin")]
        [HttpGet]
        public List<string> GetAll()
        {
            List<string> users = _userService.GetAll();
            return users;
        }

        [HttpPut("Edit")]
        public IActionResult Update(string userName, string password, UpdateRequest changeModel)
        {
            AuthenticateRequest model = new AuthenticateRequest { Username = userName, Password = password };
            ApplicationUser? user = _userService.Authenticate(model);
            if (user != null)
            {
                _userService.Update(user, changeModel);
                return Ok(new { message = "User updated successfully" });
            }
            return Ok(new { message = "User not found" });
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
         Roles = "Admin")]
        [HttpPut("RaiseToManager")]
        public IActionResult RaiseToManager(string userName)
        {
            ApplicationUser? user = _userService.GetByName(userName);
            if (user != null)
            {
                if (user.Role != "Admin")
                {
                    string role = "Manager";
                    _userService.ChangeRole(user, role);
                    return Ok(new { message = "User change role successfully" });
                }
                return Ok(new { message = "Can't change admin" });
            }
            return Ok(new { message = "User not found" });
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
         Roles = "Admin")]
        [HttpPut("FiredManager")]
        public IActionResult FiredManager(string userName)
        {
            ApplicationUser? user = _userService.GetByName(userName);
            if (user != null)
            {
                if (user.Role != "Admin")
                {
                    string role = "User";
                    _userService.ChangeRole(user, role);
                    return Ok(new { message = "Manager fired successfully" });
                }
                return Ok(new { message = "Can't change admin" });
            }
            return Ok(new { message = "User not found" });
        }
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
         Roles = "Admin")]
        [HttpDelete("byUserName")]
        public IActionResult Delete(string userName)
        {
            ApplicationUser? user = _userService.GetByName(userName);
            if (user != null)
            {
                if (user.Role != "Admin")
                {
                    _userService.Delete(user);
                    return Ok(new { message = "User deleted successfully" });
                }
                return Ok(new { message = "Can't delete user" });
            }
            return Ok(new { message = "User not found" });
        }
    }
}
