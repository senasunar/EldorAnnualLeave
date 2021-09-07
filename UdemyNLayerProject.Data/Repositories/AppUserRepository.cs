using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Data.Repositories
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        private ApplicationIdentityDbContext identityContext { get => _identityContext as ApplicationIdentityDbContext; }
        public AppUserRepository(AppDbContext context, ApplicationIdentityDbContext identityContext) : base(context, identityContext)
        {
        }
    }
}
