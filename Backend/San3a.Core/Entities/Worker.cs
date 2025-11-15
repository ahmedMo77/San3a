using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using San3a.Core.Enums;

namespace San3a.Core.Entities
{
    public class Worker
    {
        [Key]
       public int Id { get; set; }

        public bool IsVerified { get; set; } = false;
        [Required]
        public string UserId { get; set; }

       
        public double Rating { get; set; } = 0;
        public int  ServiceTypeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CompletedJobsCount { get; set; } = 0;

        #region Navigation properties
        public virtual ICollection<Offer> Offers { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<JobPost> AcceptedJobs { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }
        public  ServiceType ServiceType { get; set; }

        #endregion
       
    }
}
