using San3a.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace San3a.Core.DTOs.Job
{
    public class UpdateJobDto
    {
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string Description { get; set; }

        [StringLength(500, ErrorMessage = "Location cannot exceed 500 characters")]
        public string Location { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Budget must be greater than 0")]
        public double? Budget { get; set; }

        public JobStatus? Status { get; set; }
    }
}
