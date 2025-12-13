using San3a.Core.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace San3a.Core.DTOs.auth
{
    public class RegisterCraftsmanDto : RegisterAppUserDto
    {
        #region Properties
        [Required(ErrorMessage = "National ID is required")]
        [NationalId]
        public new string NationalId { get; set; }
        public string ServiceId { get; set; }
        #endregion
    }
}
