using San3a.Core.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace San3a.Core.Entities
{
    public class CraftsmanPortfolio : BaseAuditableEntity
    {
        #region Properties
        public string CraftsmanId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        #endregion

        #region Navigation Properties
        [ForeignKey("CraftsmanId")]
        public Craftsman Craftsman { get; set; } = null!;

        public ICollection<CraftsmanPortfolioImage> Images { get; set; } = new List<CraftsmanPortfolioImage>();
        public ICollection<PortfolioRequest> Requests { get; set; } = new List<PortfolioRequest>();
        #endregion
    }
}
