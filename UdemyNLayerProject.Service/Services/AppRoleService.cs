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
    public class AppRoleService : Service<Core.Models.AppRole>, IAppRoleService
    {
        private readonly IRepository<AppRole> _appRoleRepository;

        public AppRoleService(IUnitOfWork unitOfWork, IRepository<AppRole> repository) : base(unitOfWork, repository)
        {
            _appRoleRepository = repository;
        }

        public async Task<IEnumerable<AppRole>> GetAllRolesAsync()
        {
            return await _appRoleRepository.GetAllUsersAsync();
        }
    }
}
