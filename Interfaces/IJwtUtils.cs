using Data.Entities.IdentityClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(ApplicationUser user);
    }
}
