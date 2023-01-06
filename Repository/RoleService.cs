//using Data.Entities.IdentityClass;
//using Microsoft.AspNetCore.Identity;
//using Service.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Service
//{
//    public class RoleService : IRoleService
//    {
//        public readonly UserManager<ApplicationUser> _userManager;
//        public readonly RoleManager<ApplicationRole> _roleManager;
//        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//        }

//        ApplicationRole role1 = new ApplicationRole { Id = 1, Name = "Manager" };
//        ApplicationRole role2 = new ApplicationRole { Id = 2, Name = "Admin" };
//        public async Task AddUserToRoles()
//        {
//            await _roleManager.CreateAsync(role1);
//            await _roleManager.CreateAsync(role2);
//        }

//        public async Task CreateRoles()
//        {
//            string email1 = "ivan0202ivan@gmail.com";
//            string email2 = "ivan_kostov12_3@abv.bg";

//            var user1 = await _userManager.FindByNameAsync(email1);
//            var user2 = await _userManager.FindByNameAsync(email2);

//            await _userManager.AddToRoleAsync(user1, role1.Name);
//            await _userManager.AddToRoleAsync(user2, role2.Name);
//        }
//    }
//}
