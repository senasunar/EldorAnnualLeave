using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Data.Repositories
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        private AppDbContext appDbContext { get => _context as AppDbContext; }
        public AppUserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
