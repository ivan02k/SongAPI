using AutoMapper;
using Data;
using Data.Entities.IdentityClass;
using Microsoft.Extensions.Options;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ViewModels;

namespace Service
{
    public class UserService : IUserService
    {
        private SongContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public UserService(SongContext context, IJwtUtils jwtUtils, IMapper mapper)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public ApplicationUser? Authenticate(AuthenticateRequest model)
        {
            ApplicationUser? user = _context.Users.SingleOrDefault(x => x.Username == model.Username);
            // validate
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash)) return null;

            return user;
        }
        public AuthenticateResponse GetToken(ApplicationUser user)
        {
            AuthenticateResponse response = new AuthenticateResponse();
            // authentication successful
            response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateJwtToken(user);
            return response;
        }

        public void Delete(ApplicationUser user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public List<string> GetAll()
        {
            List<string> list = new List<string>();
            List<ApplicationUser> listOfUsers = _context.Users.ToList();
            foreach (ApplicationUser user in listOfUsers)
            {
                string json = JsonSerializer.Serialize(user).ToString();
                list.Add(json);
            }
            return list;
        }

        public ApplicationUser? GetByName(string userName)
        {
            ApplicationUser? user = _context.Users.Where(u => u.Username == userName).FirstOrDefault();
            return user;
        }

        public void Register(RegisterRequest model)
        {

            // map model to new user object
            ApplicationUser user = _mapper.Map<ApplicationUser>(model);

            // hash password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            user.Role = "User";
            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(ApplicationUser user, UpdateRequest model)
        {
            user.FirstName = (model.FirstName != "string"
                && model.FirstName != "" && model.FirstName != null) ? model.FirstName : user.FirstName;
            user.LastName = (model.LastName != "string"
                && model.LastName != "" && model.LastName != null) ? model.LastName : user.LastName;
            user.Username = (model.Username != "string"
                && model.Username != "" && model.Username != null) ? model.Username : user.Username;
            user.PasswordHash = (!string.IsNullOrEmpty(model.Password)
                && model.Password != "string"
                && model.Password != ""
                && model.Password != null) ? BCrypt.Net.BCrypt.HashPassword(model.Password) : user.PasswordHash;
            _context.SaveChanges();
        }

        public void ChangeRole(ApplicationUser user, string role)
        {
            user.Role = role;
            _context.SaveChanges();
        }
    }
}
