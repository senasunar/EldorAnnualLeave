using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Web.ViewModels
{
    public class UserViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "ID is required.")]
        [Display(Name = "ID")]
        public int Employee_ID { get; set; }

        [Required(ErrorMessage = "SAP ID is required.")]
        [Display(Name = "SAP ID")]
        public int SAP_ID { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Email Address is not in the required format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string Employee_Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [Display(Name = "Surname")]
        public string Employee_Surname { get; set; }

        [Required(ErrorMessage = "Entry Date is required.")]
        [Display(Name = "Entry Date")]
        [DataType(DataType.Date)]
        public DateTime Entry_Date { get; set; }

        public int Is_Deleted { get; set; }

        public int Is_Active { get; set; }

        public string User_Role { get; set; }
    }
}
