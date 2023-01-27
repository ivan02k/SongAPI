using Data.Entities.IdentityClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Service.Interfaces
{
    public interface IUserService
    {
        public ApplicationUser? Authenticate(AuthenticateRequest model);
        public AuthenticateResponse GetToken(ApplicationUser user);
        public List<string> GetAll();
        public ApplicationUser? GetByName(string userName);
        public void Register(RegisterRequest model);
        public void Update(ApplicationUser user, UpdateRequest model);
        public void ChangeRole(ApplicationUser user, string role);
        public void Delete(ApplicationUser user);
    }
}
