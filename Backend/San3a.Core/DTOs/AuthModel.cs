using San3a.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace San3a.Core.DTOs
{
    public class AuthModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public string Role { get; set; }
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
        public string? jwtToken { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }
    }
}
