using System.Security.Claims;
using csharp.Dto.Statistics;
using Data;
using Helper;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Repository
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ProtocolContext _context;
        
        public StatisticsRepository(ProtocolContext context)
        {
            _context = context;
        }

        public int GetNumberOfProtocols(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            var protocols = _context.Protocols.OrderByDescending(p => p.Id).Where(p => p.IsClosed == true).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.minDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) >= queryObjectStatistics.minDate);
            }

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.maxDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) <= queryObjectStatistics.maxDate);
            }

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                // var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId.ToString() == claimUserId.ToString()).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId); // || additionalUserProtocolIds.ToString().Contains(p.Id.ToString()));
            }
            else
            {
                protocols = null;
            }

            return protocols.Count();
        }

        public IEnumerable<ProtocolDateCount> GetProtocolsAggregatedByDate(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            var protocols = _context.Protocols.OrderByDescending(p => p.Id).Where(p => p.IsClosed == true).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.minDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) >= queryObjectStatistics.minDate);
            }

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.maxDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) <= queryObjectStatistics.maxDate);
            }

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                // var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId.ToString() == claimUserId.ToString()).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId); // || additionalUserProtocolIds.ToString().Contains(p.Id.ToString()));
            }
            else
            {
                protocols = null;
            }

            var dateCounts = protocols
                .GroupBy(p => DateOnly.FromDateTime(p.CreatedDate))
                .Select(group => new ProtocolDateCount
                {
                    Date = group.Key,
                    Count = group.Count()
                })
                .OrderBy(result => result.Date)
                .ToList();

            return dateCounts;
        }

        public IEnumerable<ProtocolImpactCount> GetProtocolsAggregatedByImpact(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProtocolMonthlyCount> GetProtocolsAggregatedByMonth(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            var protocols = _context.Protocols.OrderByDescending(p => p.Id).Where(p => p.IsClosed == true).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.minDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) >= queryObjectStatistics.minDate);
            }

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.maxDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) <= queryObjectStatistics.maxDate);
            }

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                // var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId.ToString() == claimUserId.ToString()).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId); // || additionalUserProtocolIds.ToString().Contains(p.Id.ToString()));
            }
            else
            {
                protocols = null;
            }

            var monthCounts = protocols
                .GroupBy(p => new { p.CreatedDate.Year, p.CreatedDate.Month })
                .Select(group => new ProtocolMonthlyCount
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    Count = group.Count()
                })
                .OrderBy(result => result.Year)
                .ThenBy(result => result.Month)
                .ToList();

            return monthCounts;
        }

        public IEnumerable<ProtocolNacaCount> GetProtocolsAggregatedByNACA(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            var protocols = _context.Protocols.OrderByDescending(p => p.Id).Where(p => p.IsClosed == true).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.minDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) >= queryObjectStatistics.minDate);
            }

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.maxDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) <= queryObjectStatistics.maxDate);
            }

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                // var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId.ToString() == claimUserId.ToString()).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId); // || additionalUserProtocolIds.ToString().Contains(p.Id.ToString()));
            }
            else
            {
                protocols = null;
            }

            var protocolIds = protocols.Select(p => p.Id);

            var protocolContents = _context.ProtocolContents.Where(p => protocolIds.Contains(p.protocolId)).ToList();
            
            var nacaCounts = protocolContents
                .SelectMany(pc =>
                {
                    try
                    {
                        // var outerJson = JsonConvert.DeserializeObject<string>(pc.Content);

                        var content = JObject.Parse(pc.Content);

                        var inputs = content["Schema"]?
                            .SelectMany(schema => schema["Inputs"])
                            .OfType<JObject>()
                            .Where(input => (string)input["Name"] == "NACA_Score" && input["Value"] != null)
                            .Select(input => (string)input["Value"]);
                        return inputs ?? Enumerable.Empty<string>();
                    }
                    catch (JsonException)
                    {

                    }
                    return Enumerable.Empty<string>();
                })
                .Where(time => !string.IsNullOrEmpty(time))
                .GroupBy(time => time)
                .Select(group => new ProtocolNacaCount
                {
                    Naca = group.Key,
                    Count = group.Count()
                })
                .OrderBy(result => result.Naca)
                .ToList();

            return nacaCounts;
        }

        public IEnumerable<ProtocolOrganizationCount> GetProtocolsAggregatedByOrganization(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            var protocols = _context.Protocols.Where(p => p.IsClosed == true).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.minDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) >= queryObjectStatistics.minDate);
            }

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.maxDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) <= queryObjectStatistics.maxDate);
            }

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                // var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId.ToString() == claimUserId.ToString()).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId); // || additionalUserProtocolIds.ToString().Contains(p.Id.ToString()));
            }
            else
            {
                protocols = null;
            }

            var orgaCounts = protocols
                .GroupBy(p => p.Organization)
                .Select(group => new ProtocolOrganizationCount
                {
                    Organization = group.Key,
                    Count = group.Count()
                })
                .ToList();

            return orgaCounts;
        }

        public IEnumerable<ProtocolPlaceCount> GetProtocolsAggregatedByPlace(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            var protocols = _context.Protocols.OrderByDescending(p => p.Id).Where(p => p.IsClosed == true).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.minDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) >= queryObjectStatistics.minDate);
            }

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.maxDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) <= queryObjectStatistics.maxDate);
            }

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                // var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId.ToString() == claimUserId.ToString()).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId); // || additionalUserProtocolIds.ToString().Contains(p.Id.ToString()));
            }
            else
            {
                protocols = null;
            }

            var protocolIds = protocols.Select(p => p.Id);

            var protocolContents = _context.ProtocolContents.Where(p => protocolIds.Contains(p.protocolId)).ToList();
            
            var placeCounts = protocolContents
                .SelectMany(pc =>
                {
                    try
                    {
                        // var outerJson = JsonConvert.DeserializeObject<string>(pc.Content);

                        var content = JObject.Parse(pc.Content);

                        var inputs = content["Schema"]?
                            .SelectMany(schema => schema["Inputs"])
                            .OfType<JObject>()
                            .Where(input => (string)input["Name"] == "Ort" && input["Value"] != null)
                            .Select(input => (string)input["Value"]);
                        return inputs ?? Enumerable.Empty<string>();
                    }
                    catch (JsonException)
                    {

                    }
                    return Enumerable.Empty<string>();
                })
                .Where(time => !string.IsNullOrEmpty(time))
                .GroupBy(time => time)
                .Select(group => new ProtocolPlaceCount
                {
                    Place = group.Key,
                    Count = group.Count()
                })
                .OrderBy(result => result.Place)
                .ToList();

            return placeCounts;
        }

        public IEnumerable<ProtocolTypeCount> GetProtocolsAggregatedByType(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            var protocols = _context.Protocols.OrderByDescending(p => p.Id).Where(p => p.IsClosed == true).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.minDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) >= queryObjectStatistics.minDate);
            }

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.maxDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) <= queryObjectStatistics.maxDate);
            }

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                // var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId.ToString() == claimUserId.ToString()).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId); // || additionalUserProtocolIds.ToString().Contains(p.Id.ToString()));
            }
            else
            {
                protocols = null;
            }

            var protocolIds = protocols.Select(p => p.Id);

            var protocolContents = _context.ProtocolContents.Where(p => protocolIds.Contains(p.protocolId)).ToList();
            
            var typeCounts = protocolContents
                .SelectMany(pc =>
                {
                    try
                    {
                        // var outerJson = JsonConvert.DeserializeObject<string>(pc.Content);

                        var content = JObject.Parse(pc.Content);

                        var inputs = content["Schema"]?
                            .FirstOrDefault(schema => (string)schema["Kategorie"] == "Einsatzart")?["Inputs"]
                            .OfType<JObject>()
                            .Where(input => (bool?)input["Value"] == true)
                            .Select(input => (string)input["Name"]);
                        return inputs ?? Enumerable.Empty<string>();
                    }
                    catch (JsonException)
                    {

                    }
                    return Enumerable.Empty<string>();
                })
                .Where(time => !string.IsNullOrEmpty(time))
                .GroupBy(time => time)
                .Select(group => new ProtocolTypeCount
                {
                    Type = group.Key,
                    Count = group.Count()
                })
                .OrderBy(result => result.Type)
                .ToList();

            return typeCounts;
        }

        public IEnumerable<ProtocolTimeCount> GetProtocolsAggregatedByTime(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            var protocols = _context.Protocols.OrderByDescending(p => p.Id).Where(p => p.IsClosed == true).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.minDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) >= queryObjectStatistics.minDate);
            }

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.maxDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) <= queryObjectStatistics.maxDate);
            }

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                // var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId.ToString() == claimUserId.ToString()).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId); // || additionalUserProtocolIds.ToString().Contains(p.Id.ToString()));
            }
            else
            {
                protocols = null;
            }

            var protocolIds = protocols.Select(p => p.Id);

            var protocolContents = _context.ProtocolContents.Where(p => protocolIds.Contains(p.protocolId)).ToList();
            
            var alarmzeitCounts = protocolContents
                .SelectMany(pc =>
                {
                    try
                    {   
                        // var outerJson = JsonConvert.DeserializeObject<string>(pc.Content);

                        var content = JObject.Parse(pc.Content);

                        var inputs = content["Schema"]?
                            .SelectMany(schema => schema["Inputs"])
                            .OfType<JObject>()
                            .Where(input => (string)input["Name"] == "Alarmzeit" && input["Value"] != null)
                            .Select(input => (string)input["Value"]);
                        return inputs ?? Enumerable.Empty<string>();
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    return Enumerable.Empty<string>();
                })
                .Where(time => !string.IsNullOrEmpty(time))
                .Select(time =>
                {
                    if (TimeSpan.TryParse(time, out var parsedTime))
                    {
                        return parsedTime;
                    }
                    return (TimeSpan?)null;
                })
                .Where(time => time.HasValue)
                .GroupBy(time => time.Value.Hours)
                .Select(group => new ProtocolTimeCount
                {
                    Time = group.Key,
                    Count = group.Count()
                })
                .OrderBy(result => result.Time)
                .ToList();

            return alarmzeitCounts;
        }

        public IEnumerable<ProtocolUserCount> GetProtocolsAggregatedByUser(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            var protocols = _context.Protocols.OrderByDescending(p => p.Id).Where(p => p.IsClosed == true)
                                    .Include(p => p.User)
                                    .Include(p => p.AdditionalUser)
                                    .ThenInclude(au => au.User)
                                    .AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.minDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) >= queryObjectStatistics.minDate);
            }

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.maxDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) <= queryObjectStatistics.maxDate);
            }

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                // var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId.ToString() == claimUserId.ToString()).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId); // || additionalUserProtocolIds.ToString().Contains(p.Id.ToString()));
            }
            else
            {
                protocols = null;
            }            

            var creatorCounts = protocols
                .GroupBy(p => p.User)
                .Select(group => new ProtocolUserCount
                {
                    User = group.Key,
                    Count = group.Count()
                })
                .ToList();

            var additionalUserCounts = protocols
                .SelectMany(p => p.AdditionalUser.Select(au => au.User))
                .GroupBy(userName => userName)
                .Select(group => new ProtocolUserCount
                {
                    User = group.Key,
                    Count = group.Count()
                })
                .ToList();

            var combinedCounts = creatorCounts
                .Concat(additionalUserCounts)
                .GroupBy(puc => puc.User)
                .Select(group => new ProtocolUserCount
                {
                    User = group.Key,
                    Count = group.Sum(puc => puc.Count)
                })
                .ToList();

            return combinedCounts;
        }

        public IEnumerable<ProtocolWeekdayCount> GetProtocolsAggregatedByWeekDay(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            var protocols = _context.Protocols.OrderByDescending(p => p.Id).Where(p => p.IsClosed == true).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.minDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) >= queryObjectStatistics.minDate);
            }

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.maxDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) <= queryObjectStatistics.maxDate);
            }

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                // var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId.ToString() == claimUserId.ToString()).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId); // || additionalUserProtocolIds.ToString().Contains(p.Id.ToString()));
            }
            else
            {
                protocols = null;
            }

            var weekdayCounts = protocols
                .GroupBy(p => new { p.CreatedDate.DayOfWeek })
                .Select(group => new ProtocolWeekdayCount
                {
                    Weekday = ((int)(group.Key.DayOfWeek + 6) % 7) + 1,
                    Count = group.Count()
                })
                .OrderBy(result => result.Weekday)
                .ToList();

            return weekdayCounts;
        }

        public IEnumerable<ProtocolYearCount> GetProtocolsAggregatedByYear(ClaimsPrincipal claimsPrincipal, QueryObjectStatistics queryObjectStatistics)
        {
            var protocols = _context.Protocols.OrderByDescending(p => p.Id).Where(p => p.IsClosed == true).AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.minDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) >= queryObjectStatistics.minDate);
            }

            if(!string.IsNullOrWhiteSpace(queryObjectStatistics.maxDate.ToString()))
            {
                protocols = protocols.Where(o => DateOnly.FromDateTime(o.CreatedDate.Date) <= queryObjectStatistics.maxDate);
            }

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                protocols = protocols;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                protocols = protocols.Where(p => claimOrganizationIds.Contains(p.Organization.Id));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                // var additionalUserProtocolIds = _context.AdditionalUsers.Where(au => au.userId.ToString() == claimUserId.ToString()).Select(p => p.protocolId).ToList();
                protocols = protocols.Where(p => p.User.Id == claimUserId); // || additionalUserProtocolIds.ToString().Contains(p.Id.ToString()));
            }
            else
            {
                protocols = null;
            }

            var yearCounts = protocols
                .GroupBy(p => new { p.CreatedDate.Year })
                .Select(group => new ProtocolYearCount
                {
                    Year = group.Key.Year,
                    Count = group.Count()
                })
                .OrderBy(result => result.Year)
                .ToList();

            return yearCounts;
        }

        public int NoOfUsersInCurrentUsersOrganizations(ClaimsPrincipal claimsPrincipal)
        {
            var users = _context.UserOrganizationRoles.AsQueryable();

            var claimRoles = claimsPrincipal.GetRoles();
            var claimOrganizationIds = claimsPrincipal.GetOrganizationIds();
            var claimUserId = claimsPrincipal.GetUserId(); 

            if (claimRoles.Contains("Admin"))
            {
                users = users;
            }
            else if (claimRoles.Contains("Leiter"))
            {
                users = users.Where(u => claimOrganizationIds.Contains(u.organizationId));
            }
            else if (claimRoles.Contains("Helfer"))
            {
                users = users.Where(u => claimOrganizationIds.Contains(u.organizationId));
            }

            return users.Select(u => u.userId).Distinct().Count();
        }
    }
}