using San3a.Core.Base;
using San3a.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace San3a.Core.Entities
{
    public class JobRequest : BaseAuditableEntity
    {
        #region Properties
        public string JobId { get; set; } = string.Empty;
        public string CraftsmanId { get; set; } = string.Empty;
        public OfferStatus Status { get; set; } = OfferStatus.Pending;
        public string? Message { get; set; }
        #endregion

        #region Navigation Properties
        [ForeignKey("JobId")]
        public Job Job { get; set; } = null!;

        [ForeignKey("CraftsmanId")]
        public Craftsman Craftsman { get; set; } = null!;
        #endregion
    }
}
