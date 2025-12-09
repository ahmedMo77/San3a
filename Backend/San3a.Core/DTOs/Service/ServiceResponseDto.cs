using San3a.Core.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.DTOs.Service
{
    public class ServiceResponseDto : BaseAuditableResponseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CraftsmenCount { get; set; }
        public int JobsCount { get; set; }
    }
}
