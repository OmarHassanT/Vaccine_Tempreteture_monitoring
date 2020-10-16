using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Models
{
    public class fridgeTempreture
    {
        public int id { get; set; }
        public float temperature { get; set; }
        public DateTime date { get; set; }
       

        public int clinicId { get; set; }
    }
}
