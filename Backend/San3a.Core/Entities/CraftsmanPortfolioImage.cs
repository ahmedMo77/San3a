using San3a.Core.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace San3a.Core.Entities
{
    public class CraftsmanPortfolioImage : BaseEntity
    {
        #region Properties
        public string PortfolioId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        #endregion

        #region Navigation Properties
        [ForeignKey("PortfolioId")]
        public CraftsmanPortfolio Portfolio { get; set; } = null!;
        #endregion
    }
}
