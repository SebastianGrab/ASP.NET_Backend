using Models;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using AutoMapper;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Helper;
using csharp.Dto.Statistics;

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

        // GET: api/statistics/number-of-users
        [HttpGet("number-of-users")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult GetNumberOfOrganizations()
        {
            var numberOfUsers = _statisticsRepository.NoOfUsersInCurrentUsersOrganizations(User);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfUsers);
        }

        // GET: api/statistics/number-of-protocols
        [HttpGet("number-of-protocols")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(int))]
        public IActionResult GetNumberOfProtocols([FromQuery] QueryObjectStatistics queryObjectStatistics = null)
        {
            var numberOfProtocols = _statisticsRepository.GetNumberOfProtocols(User, queryObjectStatistics);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }

        // GET: api/statistics/number-of-protocols-by-date
        [HttpGet("number-of-protocols-by-date")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProtocolDateCount>))]
        public IActionResult GetNumberOfProtocolsByDate([FromQuery] QueryObjectStatistics queryObjectStatistics = null)
        {
            var numberOfProtocols = _statisticsRepository.GetProtocolsAggregatedByDate(User, queryObjectStatistics);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }

        // GET: api/statistics/number-of-protocols-by-month
        [HttpGet("number-of-protocols-by-month")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProtocolMonthlyCount>))]
        public IActionResult GetNumberOfProtocolsByMonth([FromQuery] QueryObjectStatistics queryObjectStatistics = null)
        {
            var numberOfProtocols = _statisticsRepository.GetProtocolsAggregatedByMonth(User, queryObjectStatistics);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }

        // GET: api/statistics/number-of-protocols-by-year
        [HttpGet("number-of-protocols-by-year")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProtocolYearCount>))]
        public IActionResult GetNumberOfProtocolsByYear([FromQuery] QueryObjectStatistics queryObjectStatistics = null)
        {
            var numberOfProtocols = _statisticsRepository.GetProtocolsAggregatedByYear(User, queryObjectStatistics);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }

        // GET: api/statistics/number-of-protocols-by-weekday
        [HttpGet("number-of-protocols-by-weekday")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProtocolWeekdayCount>))]
        public IActionResult GetNumberOfProtocolsByWeekday([FromQuery] QueryObjectStatistics queryObjectStatistics = null)
        {
            var numberOfProtocols = _statisticsRepository.GetProtocolsAggregatedByWeekDay(User, queryObjectStatistics);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }

        // GET: api/statistics/number-of-protocols-by-alarmtime
        [HttpGet("number-of-protocols-by-alarmtime")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProtocolTimeCount>))]
        public IActionResult GetNumberOfProtocolsByTime([FromQuery] QueryObjectStatistics queryObjectStatistics = null)
        {
            var numberOfProtocols = _statisticsRepository.GetProtocolsAggregatedByTime(User, queryObjectStatistics);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }

        // GET: api/statistics/number-of-protocols-by-organization
        [HttpGet("number-of-protocols-by-organization")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProtocolOrganizationCount>))]
        public IActionResult GetNumberOfProtocolsByOrganization([FromQuery] QueryObjectStatistics queryObjectStatistics = null)
        {
            var numberOfProtocols = _statisticsRepository.GetProtocolsAggregatedByOrganization(User, queryObjectStatistics);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }

        // GET: api/statistics/number-of-protocols-by-user
        [HttpGet("number-of-protocols-by-user")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProtocolUserCount>))]
        public IActionResult GetNumberOfProtocolsByUser([FromQuery] QueryObjectStatistics queryObjectStatistics = null)
        {
            var numberOfProtocols = _statisticsRepository.GetProtocolsAggregatedByUser(User, queryObjectStatistics);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }

        // GET: api/statistics/number-of-protocols-by-type
        [HttpGet("number-of-protocols-by-type")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProtocolUserCount>))]
        public IActionResult GetNumberOfProtocolsByType([FromQuery] QueryObjectStatistics queryObjectStatistics = null)
        {
            var numberOfProtocols = _statisticsRepository.GetProtocolsAggregatedByType(User, queryObjectStatistics);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }

        // GET: api/statistics/number-of-protocols-by-place
        [HttpGet("number-of-protocols-by-place")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProtocolUserCount>))]
        public IActionResult GetNumberOfProtocolsByPlace([FromQuery] QueryObjectStatistics queryObjectStatistics = null)
        {
            var numberOfProtocols = _statisticsRepository.GetProtocolsAggregatedByPlace(User, queryObjectStatistics);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }

        // GET: api/statistics/number-of-protocols-by-naca-score
        [HttpGet("number-of-protocols-by-naca-score")]
        [Authorize(Roles = "Admin,Leiter,Helfer")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProtocolUserCount>))]
        public IActionResult GetNumberOfProtocolsByNACA([FromQuery] QueryObjectStatistics queryObjectStatistics = null)
        {
            var numberOfProtocols = _statisticsRepository.GetProtocolsAggregatedByNACA(User, queryObjectStatistics);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(numberOfProtocols);
        }
    }
}