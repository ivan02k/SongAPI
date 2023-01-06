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
        public Task LogIn(LogInViewModel obj);
        public Task Register(RegistrationViewModel obj);
    }
}
