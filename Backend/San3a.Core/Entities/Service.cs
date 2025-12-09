using San3a.Core.Base;

namespace San3a.Core.Entities
{
    public class Service : BaseAuditableEntity
    {
        #region Properties
        public string Name { get; set; }
        public string Description { get; set; }
        #endregion

        #region Navigation Properties
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
        public ICollection<Craftsman> Craftsmen { get; set; } = new List<Craftsman>();
        #endregion
    }
}