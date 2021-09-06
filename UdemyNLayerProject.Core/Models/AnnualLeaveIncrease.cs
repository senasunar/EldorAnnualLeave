using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Core.Models
{
    [Table("Eldor_AnnualLeaveIncrease")]
    public class AnnualLeaveIncrease
    {
        public int ID { get; set; }
        public int Completed_Year { get; set; }
        public int Day_Addition { get; set; }
    }
}
