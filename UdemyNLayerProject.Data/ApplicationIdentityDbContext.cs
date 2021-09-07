using EldorAnnualLeave.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Data
{
    public class ApplicationIdentityDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> dbContext) : base(dbContext)
        {

        }
    }
}
