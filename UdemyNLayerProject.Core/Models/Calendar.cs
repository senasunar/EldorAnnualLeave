using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EldorAnnualLeave.Core.Models
{
    [Table("Eldor_Calendar")]
    public class Calendar
    {
        public int ID { get; set; }
        public string Employee_ID { get; set; }
        public DateTime Start_Day { get; set; }
        public DateTime End_Day { get; set; }
        public int AnnualLeaveType_ID { get; set; }
        public int Is_Deleted { get; set; }

        [NotMapped]
        public String IDfromInput { get; set; }
    }
}
