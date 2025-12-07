using Microsoft.AspNetCore.Http.HttpResults;
using San3a.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.Entities
{
    public class Job
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public double Budget { get; set; }
        public JobStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string CustomerId { get; set; }
        public string ServiceId { get; set; } 
        public string AcceptedCraftsmanId { get; set; }


        [ForeignKey("AcceptedCraftsmanId")]
        public Craftsman AcceptedWorker { get; set; }

        [ForeignKey("ServiceId")]
        public Service serviceType { get; set; }
         
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
