using System.ComponentModel.DataAnnotations;

namespace San3a.Core.DTOs.Craftsman
{
    public class VerifyCraftsmanDto
    {
        #region Properties
        [Required]
        public string CraftsmanId { get; set; }
        
        [Required]
        public bool IsApproved { get; set; }
        
        public string? RejectionReason { get; set; }
        #endregion
    }
}
