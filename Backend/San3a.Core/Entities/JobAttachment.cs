using San3a.Core.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace San3a.Core.Entities
{
    public class JobAttachment : BaseEntity
    {
        #region Properties
        public string JobId { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        #endregion

        #region Navigation Properties
        [ForeignKey("JobId")]
        public Job Job { get; set; } = null!;
        #endregion
    }
}
