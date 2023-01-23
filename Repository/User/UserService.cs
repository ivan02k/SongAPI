using AutoMapper;
using Data;
using Data.Entities.IdentityClass;
using Microsoft.Extensions.Options;
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
        private SongContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public UserService(SongContext context, IJwtUtils jwtUtils, IMapper mapper)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            ApplicationUser? user = _context.Users.SingleOrDefault(x => x.Username == model.Username);

            // validate
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash)) return null;

            // authentication successful
            AuthenticateResponse response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateJwtToken(user);
            return response;
        }

        public void Delete(ApplicationUser user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users;
        }

        public ApplicationUser? GetById(int id)
        {
            var user = _context.Users.Find(id);
            return user;
        }

        public void Register(RegisterRequest model)
        {

            // map model to new user object
            var user = _mapper.Map<ApplicationUser>(model);

            // hash password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            user.Role = "User";
            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(ApplicationUser user, UpdateRequest model)
        {
            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // copy model to user and save
            _mapper.Map(model, user);
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
