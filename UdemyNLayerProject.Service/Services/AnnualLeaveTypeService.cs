using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Repositories;
using EldorAnnualLeave.Core.Services;
using EldorAnnualLeave.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Service.Services
{
    public class AnnualLeaveTypeService : Service<AnnualLeaveType>, IAnnualLeaveTypeService
    {
        public AnnualLeaveTypeService(IUnitOfWork unitOfWork, IRepository<AnnualLeaveType> repository) : base(unitOfWork, repository)
        {
        }
    }
}
