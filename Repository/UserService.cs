using AutoMapper;
using Data.Entities.IdentityClass;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public UserService(IMapper mapper,UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager)
        {
            this.mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task LogIn(LogInViewModel obj)
        {
            var user = await _userManager.FindByEmailAsync(obj.Email);
            string? password = obj.Password;
            if (user != null && password != null)
            {
                await _signInManager.PasswordSignInAsync(user,password,false,false);
            }
        }

        public async Task Register(RegistrationViewModel obj)
        {
            ApplicationUser newUser = mapper.Map<ApplicationUser>(obj);
            string? password = obj.Password;
            if(password != null)
            {
                var result = await _userManager.CreateAsync(newUser, password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                }
            }
        }
    }
}
