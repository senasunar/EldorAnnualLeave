using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Data.Repositories
{
    public class CalendarRepository : Repository<Calendar>, ICalendarRepository
    {
        private AppDbContext appDbContext { get => _context as AppDbContext; }
        public CalendarRepository(AppDbContext context) : base(context)
        {
        }
    }
}
