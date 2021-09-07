using EldorAnnualLeave.Core.UnitOfWorks;
using EldorAnnualLeave.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EldorAnnualLeave.Core.Repositories;
using EldorAnnualLeave.Data.Repositories;

namespace EldorAnnualLeave.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ApplicationIdentityDbContext _identityContext;

        private AnnualLeaveTypeRepository _annualLeaveTypeRepository;
        private CalendarRepository _calendarRepository;
        private EmployeeRepository _employeeRepository;
        private AnnualLeaveIncreaseRepository _annualLeaveIncreaseRepository;
        private AppRoleRepository _appRoleRepository;
        private AppUserRepository _appUserRepository;

        public IAnnualLeaveTypeRepository AnnualLeaveTypes => _annualLeaveTypeRepository = _annualLeaveTypeRepository ?? new AnnualLeaveTypeRepository(_context);

        public ICalendarRepository Calendars => _calendarRepository = _calendarRepository ?? new CalendarRepository(_context);

        public IEmployeeRepository Employees => _employeeRepository = _employeeRepository ?? new EmployeeRepository(_context);

        public IAnnualLeaveIncreaseRepository AnnualLeaveIncreases => _annualLeaveIncreaseRepository = _annualLeaveIncreaseRepository ?? new AnnualLeaveIncreaseRepository(_context);

        public IAppRoleRepository AppRoles => _appRoleRepository = _appRoleRepository ?? new AppRoleRepository(_context);

        public IAppUserRepository AppUsers => _appUserRepository = _appUserRepository ?? new AppUserRepository(_context);

        public UnitOfWork(AppDbContext appDbContext, ApplicationIdentityDbContext appIdentityDbContext)
        {
            _context = appDbContext;
            _identityContext = appIdentityDbContext;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void CommitIdentity()
        {
            _identityContext.SaveChanges();
        }

        public async Task CommitAsyncIdentity()
        {
            await _identityContext.SaveChangesAsync();
        }
    }
}