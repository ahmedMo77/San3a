using San3a.Core.Base;
using San3a.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace San3a.Core.Entities
{
    public class Offer : BaseAuditableEntity
    {
        #region Properties
        public string Message { get; set; }
        public OfferStatus Status { get; set; }
        public string JobId { get; set; }
        public string CraftsmanId { get; set; }
        #endregion

        #region Navigation Properties
        [ForeignKey("JobId")]
        public Job Job { get; set; }

        [ForeignKey("CraftsmanId")]
        public Craftsman Worker { get; set; }
        #endregion
    }
}
