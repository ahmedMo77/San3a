using San3a.Core.DTOs.Base;
using San3a.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.DTOs.Offer
{
    public class OfferResponseDto : BaseAuditableResponseDto
    {
        public string Message { get; set; }
        public OfferStatus Status { get; set; }
        public string JobId { get; set; }
        public string JobTitle { get; set; }
        public string CraftsmanId { get; set; }
        public string CraftsmanName { get; set; }
        public string CraftsmanServiceName { get; set; }
    }
}
