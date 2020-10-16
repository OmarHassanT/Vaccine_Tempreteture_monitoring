using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Models
{
    public class Directorate
    {
        public int DirectorateId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name ="Place of Directorater")]
        public string Place { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [Display(Name="Phone Number")]
        public string PhoneNumber { get; set; }

        

    }
}
