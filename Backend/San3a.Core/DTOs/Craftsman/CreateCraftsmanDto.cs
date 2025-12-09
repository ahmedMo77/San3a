using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.DTOs.Craftsman
{
    public class CreateCraftsmanDto
    {
        [Required(ErrorMessage = "National ID is required")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be 14 characters")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "Service ID is required")]
        public string ServiceId { get; set; }
    }
}
