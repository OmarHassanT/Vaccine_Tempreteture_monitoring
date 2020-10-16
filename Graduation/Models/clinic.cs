using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Models
{
    public class Clinic
    {
        public int clinicId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Place { get; set; }
        [Required]
        [Display(Name="Directorate")]
        public int? DirectorateId { get; set; }

    }
}
