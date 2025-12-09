using System.ComponentModel.DataAnnotations;

namespace San3a.Core.Base
{
    public abstract class BaseEntity
    {
        #region Properties
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        #endregion
    }
}
