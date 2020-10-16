using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Models
{
    public class ApplicationUser:IdentityUser
    {
        public int? DirectorateId { get; set; }

        public int? clinicId { get; set; }


    }
}
