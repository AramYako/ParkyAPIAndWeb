using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
    public class TokenRepository : ITokenRepository
    {
        private readonly AppSettings _appsettings;
        public TokenRepository(IOptions<AppSettings> appsettings)
        {
            this._appsettings = appsettings.Value;
        }
        public string GenerateToken(User user)
        {
            //. Issuer is “who” created this token,for example your website
            //Audience is “who” the token is supposed to be read by.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this._appsettings.Secret));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),

                    //We can also do since claim us just a freeText
                    new Claim("MyPersonalClaim","Value"),
                }),
                //Issuer= "http://mysite.com",
                //Audience = "http://myaudience.com",
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
