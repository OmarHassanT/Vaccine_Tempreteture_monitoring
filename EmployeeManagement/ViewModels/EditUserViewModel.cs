using EmployeeManagement.Utilites;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Rols = new List<string>();
            claims = new List<string>();

        }
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        [ValidEmailDomainAttribute(allowedDomain: "omar.com", ErrorMessage = "Domain must be omar.com")]
        public string Email { get; set; }
        public string City { get; set; }

        public IList<string> Rols { get; set; }
        public List<string> claims { get; set; }

    }
}
