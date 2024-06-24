using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Interfaces;
using Helper.SeachObjects;
using Dto;
using Org.BouncyCastle.OpenSsl;
using csharp.Interfaces;

namespace Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("/api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<LoginController> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IUserRepository userRepository, IRoleRepository roleRepository, IPasswordGenerator passwordGenerator, IEmailService emailService, IOrganizationRepository organizationRepository, ILogger<LoginController> logger, IMapper mapper, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _organizationRepository = organizationRepository;
            _emailService = emailService;
            _passwordGenerator = passwordGenerator;
            _roleRepository = roleRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginObject loginObject)
        {
            _logger.LogInformation(loginObject.Email + " tried to log in.");

            var loginAttemptUser = _userRepository.GetUserByEmail(loginObject.Email);
            
            if(loginAttemptUser == null)
            {
                return BadRequest(new { message = "Email or Password is incorrect." });
            }
            
            var token = _authenticationService.Login(loginObject);
            
            if(token == null || token == string.Empty || token == "Wrong username or password.")
            {
                return BadRequest(new { message = "Email or Password is incorrect." });
            }
            
            if(token == "Account is temporarily locked.")
            {
                return BadRequest(new { message = "Account is temporarily locked." });
            }

            var LoginUser = _userRepository.GetUserByEmail(loginObject.Email);
            var LoginUserOrganization = _organizationRepository.GetOrganizationsByUser(LoginUser.Id, new QueryObject(), new OrganizationSearchObject()).FirstOrDefault();
            var LoginUserRole = _roleRepository.GetRolesByUser(LoginUser.Id).FirstOrDefault();
            
            if(LoginUserOrganization == null)
            {
                return BadRequest(new { message = "no Role assigned yet." });
            }

            var LoginUserOrganizationId = (long)0;

            if (LoginUserOrganization == null)
            {
                LoginUserOrganizationId = (long)0;
            }
            else 
            {
                LoginUserOrganizationId = LoginUserOrganization.Id;
            }

            var UserRoles = _userRepository.GetUserOrganizationRoleEntriesByUser(LoginUser.Id).ToList();

            var loginReturn = new LoginReturnObject ()
            {
                Token = token,
                userId = LoginUser.Id,
                organizationId = LoginUserOrganizationId,
                role = LoginUserRole.Name.ToString(),
                uor = UserRoles
            };

            _logger.LogInformation("User with the Id " + LoginUser.Id + " logged in successfully.");

            return Ok(loginReturn);
        }

        // UPDATE: api/login/{userEmail}
        [HttpPut]
        [Route("/api/reset-password/{userEmail}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(string userEmail)
        {
            var user = _userRepository.GetUserByEmail(userEmail);

            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            user.Password = _passwordGenerator.GetRandomAlphanumericString(12);

            if(!_emailService.SendResetEmail(user.FirstName + " " + user.LastName, user.Email, user.Password))
            {
                ModelState.AddModelError("", "Something went wrong sending the mail.");
                return StatusCode(301, ModelState);
            }

            var userMap = _mapper.Map<User>(user);
            
            userMap.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // SALT is created automatically by the method.
            userMap.Email = user.Email.ToLower();

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating User.");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}