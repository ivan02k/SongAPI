using Data.Entities.IdentityClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DataSeeder
    {
        public readonly SongContext _context;
        public DataSeeder(SongContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            //Seed Users
            if(_context.Users.Count() == 0)
            {
                List<ApplicationUser> defaultUsers = new List<ApplicationUser>();

                defaultUsers.Add(new ApplicationUser()
                {
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    Username = "Admin",
                    Role = "Admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ivan123456")
                });

                _context.Users.AddRange(defaultUsers);
                _context.SaveChanges();
            }
        }
    }
}
