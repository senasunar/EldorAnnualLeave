using EldorAnnualLeave.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Core.Services
{
    public interface IAnnualLeaveTypeService : IService<AnnualLeaveType>
    {
        Task<List<AnnualLeaveType>> CreateLeaveTypeTable();
    }
}
