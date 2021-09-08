using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Web.ViewModels
{
    public class UpdatePasswordViewModel
    {
        [Required(ErrorMessage = "Current password is required.")]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "New password again is required.")]
        [Display(Name = "New Password Again")]
        [DataType(DataType.Password)]
        public string PasswordAgain { get; set; }

        public string Id { get; set; }
    }
}
