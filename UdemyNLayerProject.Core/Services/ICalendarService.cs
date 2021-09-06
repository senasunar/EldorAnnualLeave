using EldorAnnualLeave.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Core.Services
{
    public interface ICalendarService : IService<Calendar>
    {
        public Task<List<Calendar>> CreateEmployeeCalendarTable(int employeeID);
    }
}
