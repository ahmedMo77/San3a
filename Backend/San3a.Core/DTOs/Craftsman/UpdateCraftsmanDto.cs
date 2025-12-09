using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.DTOs.Craftsman
{
    public class UpdateCraftsmanDto
    {
        [Required]
        public string ServiceId { get; set; }
        
        public bool? IsVerified { get; set; }
    }
}
