﻿using Graduation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.ViewModels
{
    public class EditViewModel
    {
        public EditViewModel()
        {
            Directorates = new List<Directorate>();


        }

        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "current password")]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]

        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [Display(Name = "Confirm New password")]
        public string ConfirmPassword { get; set; }
        [Required]

        public List<Directorate> Directorates { get; set; }


        [Display(Name = "Directorate")]
        public int? DirctorateId { get; set; }
        [Display(Name = "Clinic")]
        public int? ClinicId { get; set; }

    }
}
