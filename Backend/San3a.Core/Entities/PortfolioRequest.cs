using San3a.Core.Base;
using San3a.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace San3a.Core.Entities
{
    public class PortfolioRequest : BaseAuditableEntity
    {
        #region Properties
        public string PortfolioId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public OfferStatus Status { get; set; } = OfferStatus.Pending;
        public string? Message { get; set; }
        #endregion

        #region Navigation Properties
        [ForeignKey("PortfolioId")]
        public CraftsmanPortfolio Portfolio { get; set; } = null!;

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } = null!;
        #endregion
    }
}
