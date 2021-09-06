using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EldorAnnualLeave.API.DTOs
{
    public class AnnualLeaveIncreaseDto
    {
        public int Completed_Year { get; set; }
        public int Day_Addition { get; set; }
    }
}
