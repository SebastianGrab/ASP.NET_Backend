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
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IUserRepository userRepository, IOrganizationRepository organizationRepository, ILogger<LoginController> logger, IMapper mapper, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _organizationRepository = organizationRepository;
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
            
            if(token == null || token == string.Empty)
            {
                return BadRequest(new { message = "Email or Password is incorrect." });
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
    }
}