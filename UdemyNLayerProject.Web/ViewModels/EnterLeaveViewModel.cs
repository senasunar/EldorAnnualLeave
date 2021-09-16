using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Web.ViewModels
{
    public class EnterLeaveViewModel
    {
        [Display(Name = "Start Day")]
        [Required(ErrorMessage = "Start Day is required")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Start_Day { get; set; }

        [Display(Name = "End Day")]
        [Required(ErrorMessage = "End Day is required")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime End_Day { get; set; }

        [Display(Name = "Annual Leave Type")]
        [Required(ErrorMessage = "Annual Leave Type is required")]
        public String annualLeaveTypeList { get; set; }

        public String Employee_ID { get; set; }

        public int Calendar_ID { get; set; }

        public List<SelectListItem> ALselectList { set; get; }
    }
}
