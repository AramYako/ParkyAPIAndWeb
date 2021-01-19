using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyApi.Data;
using ParkyApi.Models.Instances;
using ParkyApi.Models.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkyApi.Models.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ParkyDbContext _db;
        private readonly AppSettings _appsettings;

        public UserRepository(ParkyDbContext db, IOptions<AppSettings> appsettings)
        {
            this._db = db;
            this._appsettings = appsettings.Value;
        }
        public User Authenticate(string username, string password)
        {
            User user = this._db.Users.SingleOrDefault(u => u.Username.Equals(username) && u.Password.Equals(password));

            if (user == null)
                return null;

            user.Password = "";

            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var user = this._db.Users.Where(u => u.Username.Equals(username)).FirstOrDefault();

            if (user == null)
                return true;
            else
                return false; 
        }

        public User Register(string username, string password)
        {
            User user = new User()
            {
                Username = username,
                Password = password,
                Role = "Admin"
            };

            this._db.Add(user);
            this._db.SaveChanges();

            user.Password = "";

            return user;

        }
    }
}
