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
    public class AppUserService : Service<Core.Models.AppUser>, IAppUserService
    {
        public AppUserService(IUnitOfWork unitOfWork, IRepository<Core.Models.AppUser> repository) : base(unitOfWork, repository)
        {
        }
    }
}
