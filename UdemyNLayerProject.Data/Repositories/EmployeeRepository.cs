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
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private AppDbContext appDbContext { get => _context as AppDbContext; }
        public EmployeeRepository(AppDbContext context, ApplicationIdentityDbContext identityContext) : base(context, identityContext)
        {
        }

        public void CalculateUsed(Employee employee)
        {
            int usedLeave = 0;

            foreach (var leaveDate in employee.Calendar)
            {
                TimeSpan ts = DateTime.Now.Subtract(leaveDate.End_Day);
                if (ts.TotalDays >= 0) usedLeave += ((int)ts.TotalDays);
            }
            employee.usedLeave = usedLeave;
            _context.Entry(employee).State = EntityState.Modified;
        }

        public void CalculateRest(Employee employee)
        {
            int restLeave = 0;

            foreach (var leaveDate in employee.Calendar)
            {
                TimeSpan ts = DateTime.Now.Subtract(leaveDate.End_Day);
                if (ts.TotalDays < 0) restLeave -= ((int)ts.TotalDays);
            }

            employee.restOfLeave = restLeave;
            _context.Entry(employee).State = EntityState.Modified;
        }

        public void CalculateTotal(Employee employee)
        {
            int total = employee.usedLeave + employee.restOfLeave;
            employee.totalLeave = total;
            _context.Entry(employee).State = EntityState.Modified;
        }
    }
}
