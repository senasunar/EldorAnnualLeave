using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Data.Repositories
{
    public class AppRoleRepository : Repository<AppRole>, IAppRoleRepository
    {
        private ApplicationIdentityDbContext identityContext { get => _identityContext as ApplicationIdentityDbContext; }
        public AppRoleRepository(AppDbContext context, ApplicationIdentityDbContext identityContext) : base(context, identityContext)
        {
        }
    }
}
