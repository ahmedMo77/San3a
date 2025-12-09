using San3a.Core.DTOs.Base;

namespace San3a.Core.DTOs.Portfolio
{
    public class PortfolioImageDto : BaseResponseDto
    {
        public string PortfolioId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
