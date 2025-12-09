using San3a.Core.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace San3a.Core.DTOs
{
    public class RegisterAppUserDto
    {
        #region Properties
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        
        [NationalId]
        public string? NationalId { get; set; }
        #endregion
    }
}
