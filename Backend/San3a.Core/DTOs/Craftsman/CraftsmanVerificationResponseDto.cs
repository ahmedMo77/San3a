using San3a.Core.DTOs.Base;

namespace San3a.Core.DTOs.Craftsman
{
    public class CraftsmanVerificationResponseDto
    {
        #region Properties
        public string CraftsmanId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public string ServiceName { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        #endregion
    }
}
