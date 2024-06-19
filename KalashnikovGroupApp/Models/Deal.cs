using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalashnikovGroupApp.Models
{
    internal class Deal
    {
        public int id_deal { get; set; }
        public DateTime date { get; set; }
        public int quality { get; set; }
        public float total_amount { get; set; }
        public int Employeesid_employess { get; set; }
        public int Operationsid_operations { get; set; }
    }
}
