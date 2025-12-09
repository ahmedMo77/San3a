using San3a.Core.DTOs.Base;
using San3a.Core.Enums;

namespace San3a.Core.DTOs.Portfolio
{
    public class PortfolioRequestResponseDto : BaseAuditableResponseDto
    {
        public string PortfolioId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public OfferStatus Status { get; set; }
        public string? Message { get; set; }
        public PortfolioResponseDto? Portfolio { get; set; }
    }
}
