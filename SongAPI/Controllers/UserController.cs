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
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
        Roles = "Admin")]
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
            var response = _userService.Authenticate(model);
            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _userService.Register(model);
            return Ok(new { message = "Registration successful" });
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateRequest model)
        {
            ApplicationUser? user = _userService.GetById(id);
            if (user != null)
            {
                _userService.Update(user, model);
                return Ok(new { message = "User updated successfully" });
            }
            return Ok(new { message = "User not found" });
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            ApplicationUser? user = _userService.GetById(id);
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
