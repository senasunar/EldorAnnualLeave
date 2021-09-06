using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EldorAnnualLeave.Core.Repositories;

namespace EldorAnnualLeave.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IAnnualLeaveTypeRepository AnnualLeaveTypes { get; }
        ICalendarRepository Calendars { get; }
        IEmployeeRepository Employees { get; }
        ILeaveAdditionRepository LeaveAdditions { get; }
        IAnnualLeaveIncreaseRepository AnnualLeaveIncreases { get; }

        Task CommitAsync();

        void Commit();
    }
}