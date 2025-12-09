using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.DTOs.Base
{
    public class BaseAuditableResponseDto : BaseResponseDto
    {
        public DateTime UpdatedAt { get; set; }
    }
}
