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

namespace Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("/api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public LoginController(ILogger<LoginController> logger, IMapper mapper, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginObject loginObject)
        {
            var token = _authenticationService.Login(loginObject);
            if(token == null || token == string.Empty)
            {
                return BadRequest(new { message = "Email or Password is incorrect." });
            }
            return Ok(token);
        }
    }
}