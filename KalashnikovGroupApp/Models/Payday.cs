using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalashnikovGroupApp.Models
{
    internal class Payday
    {
        public int id_payday { get; set; }
        public float paycheck { get; set; }
        public DateTime end_date { get; set; }
        public DateTime start_date { get; set; }
    }
}
