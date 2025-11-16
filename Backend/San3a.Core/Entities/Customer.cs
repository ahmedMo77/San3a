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
    public class Customer
    {
        [Key]
        public int Id {  get; set; }

        [Required]
        public string UserId { get; set; }

       
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
      
        #region Navigation properties
        public virtual ICollection<JobPost> JobPosts { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Worker> SavedWorkers { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }
        #endregion


        
    }
}

