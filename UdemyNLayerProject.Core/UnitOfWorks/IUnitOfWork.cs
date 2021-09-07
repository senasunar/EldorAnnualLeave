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
        IAnnualLeaveIncreaseRepository AnnualLeaveIncreases { get; }
        IAppRoleRepository AppRoles { get; }
        IAppUserRepository AppUsers { get; }

        Task CommitAsync();
        Task CommitAsyncIdentity();

        void CommitIdentity();
        void Commit();
    }
}