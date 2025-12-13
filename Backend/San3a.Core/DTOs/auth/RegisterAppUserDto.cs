using San3a.Core.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace San3a.Core.DTOs.auth
{
    public class RegisterAppUserDto
    {
        #region Properties
        public string FullName { get; set; }
        public string Governorate {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        
        [NationalId]
        public string? NationalId { get; set; }
        #endregion
    }
}
