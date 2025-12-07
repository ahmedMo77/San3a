using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.Entities
{
    public class Admin
    {
        [Key]
        public string Id { get; set; }    // <-- This is AppUserId (shared PK)
        public bool IsSuperAdmin { get; set; }

        public AppUser AppUser { get; set; }
    }
}
