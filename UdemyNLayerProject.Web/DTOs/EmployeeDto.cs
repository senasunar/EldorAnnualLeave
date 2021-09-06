using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Web.DTOs
{
    public class EmployeeDto
    {
        public int Employee_ID { get; set; }
        public int SAP_ID { get; set; }
        public string Employee_Email { get; set; }
        public string Employee_Password { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_Surname { get; set; }
        public DateTime Entry_Date { get; set; }
        public int Annual_Leave { get; set; }
        public DateTime Last_Calculation { get; set; }
    }
}
