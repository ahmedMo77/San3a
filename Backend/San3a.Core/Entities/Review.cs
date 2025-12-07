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
    public class Review
    {
        [Key]
        public string Id { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public ReviewType Type { get; set; }

        public string ReviewerId { get; set; }
        public string RevieweeId { get; set; }


        [ForeignKey("ReviewerId")]
        public AppUser Reviewer { get; set; }

        [ForeignKey("RevieweeId")]
        public AppUser Reviewee { get; set; }
    }
}
