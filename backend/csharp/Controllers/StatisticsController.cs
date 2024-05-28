using Models;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IMapper _mapper;

        public StatisticsController(IStatisticsRepository statisticsRepository, IMapper mapper)
        {
            _statisticsRepository = statisticsRepository;
            _mapper = mapper;
        }

        // GET: api/statistics/number-of-organizations
        [HttpGet("number-of-organizations")]
        [Authorize(Roles = "Admin,Leiter")]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult GetNumberOfOrganizations()
        {
            var numberOfOrganizations = _statisticsRepository.GetNumberOfOrganizations();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfOrganizations);
        }

        // GET: api/statistics/number-of-protocols
        [HttpGet("number-of-protocols")]
        [Authorize(Roles = "Admin,Leiter")]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult GetNumberOfProtocols()
        {
            var numberOfProtocols = _statisticsRepository.GetNumberOfProtocols();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }

        // GET: api/statistics/number-of-protocols-per-user/{userId}
        [HttpGet("number-of-protocols-per-user/{userId}")]
        [Authorize(Roles = "Admin,Leiter")]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult GetNumberOfProtocolsPerUser(long userId)
        {
            var numberOfProtocolsPerUser = _statisticsRepository.GetNumberOfProtocolsPerUser(userId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocolsPerUser);
        }
    }
}