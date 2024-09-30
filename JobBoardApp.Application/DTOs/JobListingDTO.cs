using JobBoardApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoardApp.Application.DTOs
{
    public class JobListingDTO
    {
        public Guid Id { get; set; }
        public string EmployerId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public decimal SalaryRange { get; set; }
        [Required]
        public JobType JobType { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsActive { get; set; }

        public string EmployerName { get; set; }
    }
}
