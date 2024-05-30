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
        private readonly ILogger<LoginController> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IUserRepository userRepository, IPasswordGenerator passwordGenerator, IEmailService emailService, IOrganizationRepository organizationRepository, ILogger<LoginController> logger, IMapper mapper, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _organizationRepository = organizationRepository;
            _emailService = emailService;
            _passwordGenerator = passwordGenerator;
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

            var loginReturn = new LoginReturnObject ()
            {
                Token = token,
                userId = LoginUser.Id,
                organizationId = LoginUserOrganization.Id
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

            var updateUser = new UserRegisterDto() 
            {
                Email = user.Email,
                Password = _passwordGenerator.GetRandomAlphanumericString(12),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id
            };

            var userMap = _mapper.Map<User>(updateUser);
            
            userMap.Password = BCrypt.Net.BCrypt.HashPassword(updateUser.Password); // SALT is created automatically by the method.
            userMap.Email = updateUser.Email.ToLower();

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating User.");
                return StatusCode(500, ModelState);
            }

            if(!_emailService.SendResetEmail(updateUser.FirstName + " " + updateUser.LastName, updateUser.Email, updateUser.Password))
            {
                ModelState.AddModelError("", "Saving was successful. Something went wrong sending the mail.");
                return StatusCode(301, ModelState);
            }

            var userToReturn = _mapper.Map<UserDto>(userMap);

            return Ok(userToReturn);
        }
    }
}