using San3a.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace San3a.Core.DTOs
{
    public class RegisterModel
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be 14 digits.")]
        public string NationalId { get; set; }

        [Required, MaxLength(30)]
        public string City { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string? ImgUrl { get; set; }
        public string? Address { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
