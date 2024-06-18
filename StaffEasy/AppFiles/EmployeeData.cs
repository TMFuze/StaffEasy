using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffEasy.AppFiles
{
    public class EmployeeData
    {
        public int EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
