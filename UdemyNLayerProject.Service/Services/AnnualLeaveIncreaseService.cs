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
    public class AnnualLeaveIncreaseService : Service<Core.Models.AnnualLeaveIncrease>, IAnnualLeaveIncreaseService
    {
        public AnnualLeaveIncreaseService(IUnitOfWork unitOfWork, IRepository<Core.Models.AnnualLeaveIncrease> repository) : base(unitOfWork, repository)
        {
        }
    }
}
