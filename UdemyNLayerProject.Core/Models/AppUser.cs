using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Core.Models
{
    public class AppUser : IdentityUser
    {
        public int Employee_ID { get; set; }
        public int SAP_ID { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_Surname { get; set; }
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
