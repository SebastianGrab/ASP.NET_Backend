using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace csharp.Dto.Statistics
{
    public class ProtocolOrganizationCount
    {
        public required Organization Organization { get; set; }
        public int Count { get; set; }        
    }
}