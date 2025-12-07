using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.DTOs
{
    public class RegisterAppUserDto
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
