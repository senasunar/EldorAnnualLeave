using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Web.ViewModels
{
    public class UpdateAllViewModel
    {
        [Display(Name = "Users")]
        public string Id { get; set; }
    }
}
