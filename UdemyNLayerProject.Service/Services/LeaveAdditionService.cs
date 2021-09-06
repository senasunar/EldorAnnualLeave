using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Repositories;
using EldorAnnualLeave.Core.Services;
using EldorAnnualLeave.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Service.Services
{
    public class LeaveAdditionService : Service<LeaveAddition>, ILeaveAdditionService
    {
        public LeaveAdditionService(IUnitOfWork unitOfWork, IRepository<LeaveAddition> repository) : base(unitOfWork, repository)
        {
        }
    }
}
