using San3a.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.Entities
{
    public class JobPost
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        
        public int CustomerId { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Pending;

        public double Budget { get; set; } 
        
        public string Location { get; set; }
        
        public int ServiceId { get; set; }
        
        public PostingType PostingType { get; set; }
        
        public int AcceptedWorkerId { get; set; }

        #region Navigation properties
        [ForeignKey("AcceptedWorkerId")]
        public Worker AcceptedWorker { get; set; }

        [ForeignKey("ServiceId")]
        public ServiceType serviceType { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public virtual ICollection<Offer> Offers { get; set; } 
        #endregion

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
