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
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<ApplicationUser> GetAll();
        ApplicationUser? GetById(int id);
        void Register(RegisterRequest model);
        void Update(ApplicationUser user, UpdateRequest model);
        void Delete(ApplicationUser user);
    }
}
