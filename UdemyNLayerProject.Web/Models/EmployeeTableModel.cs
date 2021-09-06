using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Web.Models
{
    public class EmployeeTableModel
    {
        public int Employee_ID { get; set; }
        public int Annual_Leave { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_Surname { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Entry_Date { get; set; }

        public int Planned_Leave { get; set; }
        public int Rest_Of_Leave { get; set; }
        public int Used_Leave { get; set; }
        public int Total_Leave { get; set; }
    }
}
