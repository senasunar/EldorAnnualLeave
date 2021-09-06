using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EldorAnnualLeave.API.DTOs
{
    public class LeaveAdditionDto
    {
        public int AnnualLeaveType_ID { get; set; }
        public int Completed_Years { get; set; }
        public int Added_Annual_Leave { get; set; }
    }
}
