using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace csharp.Dto.Statistics
{
    public class ProtocolUserCount
    {
        public required User User { get; set; }
        public int Count { get; set; }
    }
}