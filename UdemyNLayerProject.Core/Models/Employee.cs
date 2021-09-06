using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EldorAnnualLeave.Core.Models
{
    [Table("AspNetUsers")]
    public class Employee
    {
        public String ID { get; set; }
        public int Employee_ID { get; set; }
        public int SAP_ID { get; set; }
        public String Email { get; set; }
        public String UserName { get; set; }
        public String PasswordHash { get; set; }
        public String Employee_Name { get; set; }
        public String Employee_Surname { get; set; }
        public DateTime Entry_Date { get; set; }
        public int Is_Deleted { get; set; }
        public int Is_Active { get; set; }
        [NotMapped]
        public List<Calendar> Calendar { get; set; }

        [NotMapped]
        public int totalLeave { get; set; }

        [NotMapped]
        public int usedLeave { get; set; }

        [NotMapped]
        public int plannedLeave { get; set; }

        [NotMapped]
        public int restOfLeave { get; set; }

        [NotMapped]
        public int annualLeave { get; set; }
    }
}

//AQAAAAEAACcQAAAAEAaHqGaDIFxJppSbLWzJqoYEsqxtY6sv7cPb5GiE7mYguiJLJQkaLhpI89wpBQUBbw==