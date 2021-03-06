using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using EldorAnnualLeave.Core.Models;

namespace EldorAnnualLeave.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AnnualLeaveType> AnnualLeaveTypes { get; set; }

        public DbSet<Calendar> Calendars { get; set; }

        public DbSet<AnnualLeaveIncrease> AnnualLeaveIncreases { get; set; }
    }
}