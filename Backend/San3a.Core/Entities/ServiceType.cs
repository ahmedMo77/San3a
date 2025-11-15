using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.Entities
{
    public class ServiceType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public String Description { get; set; }

        public bool IsActive { get; set; } = true;


        #region Navigation properties

        public ICollection<JobPost> JobPosts { get; set; } 
        public ICollection<Worker> workers { get; set; }
        #endregion

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        


    }
}
