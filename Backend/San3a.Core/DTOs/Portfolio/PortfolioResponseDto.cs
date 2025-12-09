using San3a.Core.DTOs.Base;

namespace San3a.Core.DTOs.Portfolio
{
    public class PortfolioResponseDto : BaseAuditableResponseDto
    {
        public string CraftsmanId { get; set; } = string.Empty;
        public string CraftsmanName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<PortfolioImageDto> Images { get; set; } = new();
    }
}
