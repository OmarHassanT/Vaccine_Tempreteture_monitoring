using Graduation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.ViewModels
{
    public class ClinicFridgeViewModel
    {
       public Task<IEnumerable<fridgeTempreture>> FridgeData { get; set; }
        public string ClinicName { get; set; }
    }
}
