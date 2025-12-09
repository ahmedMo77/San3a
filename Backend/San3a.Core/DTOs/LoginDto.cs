using System.ComponentModel.DataAnnotations;

namespace San3a.Core.DTOs
{
    public class LoginDto
    {
        #region Properties
        [Required(ErrorMessage = "Email or Full Name is required")]
        public string EmailOrUsername { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        #endregion
    }
}
