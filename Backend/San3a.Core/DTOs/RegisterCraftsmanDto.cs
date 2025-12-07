using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.DTOs
{
    public class RegisterCraftsmanDto : RegisterAppUserDto
    {
        public string NationalId { get; set; }
        public string ServiceId { get; set; }
    }
}
