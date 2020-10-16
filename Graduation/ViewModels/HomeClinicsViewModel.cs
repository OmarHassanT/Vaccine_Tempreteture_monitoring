using Graduation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.ViewModels
{
    public class HomeClinicsViewModel
    {
       public IEnumerable<Clinic> Clinics { get; set; }
       public string pageTitle { get; set; }
    }
}
