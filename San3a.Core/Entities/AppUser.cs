using Microsoft.AspNetCore.Identity;
using San3a.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace San3a.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public string City { get; set; }
        public string? Address { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsApproved { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
