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
        private readonly IUserLoginAttemptRepository _userLoginAttemptRepository;

        public AuthenticationService(IConfiguration configuration, IUserRepository userRepository, IUserLoginAttemptRepository userLoginAttemptRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _userLoginAttemptRepository = userLoginAttemptRepository;
        }

        public string Login(LoginObject loginObject)
        {
            User LoginUser = _userRepository.GetUsers(new QueryObject(), new UserSearchObject()).Where(x => x.Email.ToLower() == loginObject.Email.ToLower()).FirstOrDefault();

            if(LoginUser == null)
            {
                Thread.Sleep(5000); 
                return "User does not exist.";
            }
            
            UserLoginAttempt currentUserLoginAttempt = _userLoginAttemptRepository.GetUserLoginAttempt(LoginUser.Id);

            if (currentUserLoginAttempt.FailedLoginAttempts >= 10 &&
                currentUserLoginAttempt.LastLoginAttempt.AddMinutes(15) > DateTime.UtcNow)
            {
                Thread.Sleep(10000);
                return "Account is temporarily locked.";
            }

            if(!_userRepository.VerifyUserPassword(LoginUser, loginObject.Password))
            {
                currentUserLoginAttempt.FailedLoginAttempts += 1;
                currentUserLoginAttempt.LastLoginAttempt = DateTime.UtcNow;
                _userLoginAttemptRepository.UpdateUserLoginAttempt(currentUserLoginAttempt);
                Thread.Sleep(5000);
                return "Wrong username or password.";
            }

            // Reset failed login attempts on successful login
            currentUserLoginAttempt.FailedLoginAttempts = 0;
            currentUserLoginAttempt.LastLoginAttempt = DateTime.UtcNow;
            _userLoginAttemptRepository.UpdateUserLoginAttempt(currentUserLoginAttempt);

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Subject = new ClaimsIdentity(new[]
                // {
                //     new Claim("Id", Guid.NewGuid().ToString()),
                //     new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                //     new Claim(JwtRegisteredClaimNames.Email, user.Email),
                //     new Claim(JwtRegisteredClaimNames.Jti,
                //     Guid.NewGuid().ToString())
                // }),
                Expires = DateTime.UtcNow.AddMinutes(90),
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