using San3a.Core.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.DTOs.Customer
{
    public class CustomerResponseDto : BaseResponseDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Governorate { get; set; }
        public string ProfileImageUrl { get; set; }
        public int JobsCount { get; set; }
        public int ReviewsCount { get; set; }
    }
}
