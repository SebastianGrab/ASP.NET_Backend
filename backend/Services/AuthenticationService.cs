using Helper;
using Helper.SearchObjects;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models;
using Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public string Login(LoginObject loginObject)
        {
            User LoginUser = _userRepository.GetUsers(new QueryObject(), new UserSearchObject()).Where(x => x.Email == loginObject.Email).FirstOrDefault();

            if(LoginUser == null)
            {
                return string.Empty;
            }

            if(!_userRepository.VerifyUserPassword(LoginUser, loginObject.Password))
            {
                return string.Empty;
            }

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Subject = new ClaimsIdentity(new[]
                // {
                //     new Claim("Id", Guid.NewGuid().ToString()),
                //     new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                //     new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                //     new Claim(JwtRegisteredClaimNames.Jti,
                //     Guid.NewGuid().ToString())
                // }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
    }
}