using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Core.Models
{
    [Table("Eldor_LeaveAddition")]
    public class LeaveAddition
    {
        public int ID { get; set; }
        public int AnnualLeaveType_ID { get; set; }
        public int Completed_Years { get; set; }
        public int Added_Annual_Leave { get; set; }
    }
}
