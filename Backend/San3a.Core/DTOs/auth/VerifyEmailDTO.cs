using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.DTOs.auth
{
    public class VerifyEmailDTO
    {
       public string Email { get; set; }
        public string code { get; set; }

    }
}
