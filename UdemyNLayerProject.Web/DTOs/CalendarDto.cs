using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Web.DTOs
{
    public class CalendarDto
    {
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Start_Day { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime End_Day { get; set; }

        public string annualLeaveType { get; set; }
        public int total { get; set; }
        public int ID { get; set; }
    }
}
