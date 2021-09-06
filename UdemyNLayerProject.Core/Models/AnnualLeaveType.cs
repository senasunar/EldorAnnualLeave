using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EldorAnnualLeave.Core.Models
{
    [Table("Eldor_AnnualLeaveType")]
    public class AnnualLeaveType
    {
        public int ID { get; set; }
        public string ALT_Name { get; set; }
        public string ALT_Color { get; set; }
        public int Is_Active { get; set; }
        public int Is_Deleted { get; set; }
    }
}
