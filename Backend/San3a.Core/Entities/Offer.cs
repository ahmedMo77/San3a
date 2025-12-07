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
    public class Offer
    {
        [Key]
        public string Id { get; set; }
        public  string Message { get; set; }
        public OfferStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string JobId { get; set; }
        public string CraftsmanId { get; set; }


        [ForeignKey("JobId")]
        public  Job Job { get; set; }

        [ForeignKey("CraftsmanId")]
        public Craftsman Worker { get; set; }
    }
}
