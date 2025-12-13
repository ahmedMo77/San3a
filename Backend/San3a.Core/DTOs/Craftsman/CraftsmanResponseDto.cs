using San3a.Core.DTOs.Base;

namespace San3a.Core.DTOs.Craftsman
{
    public class CraftsmanResponseDto : BaseResponseDto
    {
        #region Properties
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
         public string Governorate { get; set; }
        public string NationalId { get; set; }
        public bool IsVerified { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ProfileImageUrl { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public int CompletedJobsCount { get; set; }
        #endregion
    }
}
