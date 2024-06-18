using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalashnikovGroupApp.Models
{
    internal class Employees
    {
        public int id_employess { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string middlename { get; set; }
        public float wage_rate { get; set; }
        public int Postid_post { get; set; }
    }
}
