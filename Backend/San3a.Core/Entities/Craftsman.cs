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
    public class Craftsman
    {
        [Key]
        public string Id { get; set; }
        public string NationalId { get; set; }
        public bool IsVerified { get; set; }
        public string ServiceId { get; set; }

        public AppUser AppUser { get; set; }
        public Service Service { get; set; }

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Job> AcceptedJobs { get; set; } = new List<Job>();
    }

}
