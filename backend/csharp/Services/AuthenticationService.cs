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
using MailKit.Search;
using Helper.SeachObjects;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserLoginAttemptRepository _userLoginAttemptRepository;

        public AuthenticationService(IConfiguration configuration, IOrganizationRepository organizationRepository, IUserRepository userRepository, IRoleRepository roleRepository, IUserLoginAttemptRepository userLoginAttemptRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _organizationRepository = organizationRepository;
            _userLoginAttemptRepository = userLoginAttemptRepository;
        }

        public string Login(LoginObject loginObject)
        {
            User LoginUser = _userRepository.GetUserByEmail(loginObject.Email.ToLower());

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

            
            var claims = new List<Claim>
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, LoginUser.Username),
                new Claim(JwtRegisteredClaimNames.Email, LoginUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, LoginUser.Id.ToString()),
                new Claim("UserId", LoginUser.Id.ToString())
            };

            var roles = _roleRepository.GetRolesByUser(LoginUser.Id);

            foreach (var role in roles.Distinct())
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var organizations = _organizationRepository.GetOrganizationsByUser(LoginUser.Id, new QueryObject(), new OrganizationSearchObject());

            List<string> organizationIds = new List<string> {};

            foreach (var organization in organizations)
            {
                organizationIds.Add(organization.Id.ToString());
                organizationIds.Distinct();
            }

            foreach (var organization in organizations)
            {
                var daughterOrgas = _organizationRepository.GetAllOrganizationDaughters(organization.Id, new QueryObject(), new OrganizationSearchObject());
                if (daughterOrgas != null) 
                {
                    foreach (var daughter in daughterOrgas)
                    {
                        organizationIds.Add(daughter.Id.ToString());
                    }
                }
                organizationIds.Distinct();
            }


            foreach (var organizationId in organizationIds)
            {
                claims.Add(new Claim("Organization", organizationId));
            }


            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
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