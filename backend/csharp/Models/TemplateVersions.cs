using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace csharp.Models
{
    public class TemplateVersions : ITimestampEntity
    {
        public long Id { get; set; }

        public string TemplateContent { get; set; }

        public long templateId { get; set; }

        public Template Template { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; } 
        
    }
}