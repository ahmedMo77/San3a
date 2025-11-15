using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using San3a.Core.Enums;

namespace San3a.Core.Entities
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int JobPostId { get; set; }

        [ForeignKey("JobPostId")]
        public virtual JobPost JobPost { get; set; }

        [Required]
        public int WorkerId { get; set; }

        [ForeignKey("WorkerId")]
        public Worker Worker { get; set; }

        [Required]
        public double ProposedPrice { get; set; }

        [MaxLength(100)]
        public string ProposedTimeline { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public OfferStatus Status { get; set; } = OfferStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    }
}
