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
        private AnnualLeaveIncreaseRepository _annualLeaveIncreaseRepository;
        private AppRoleRepository _appRoleRepository;
        private AppUserRepository _appUserRepository;

        public IAnnualLeaveTypeRepository AnnualLeaveTypes => _annualLeaveTypeRepository = _annualLeaveTypeRepository ?? new AnnualLeaveTypeRepository(_context, _identityContext);

        public ICalendarRepository Calendars => _calendarRepository = _calendarRepository ?? new CalendarRepository(_context, _identityContext);

        public IAnnualLeaveIncreaseRepository AnnualLeaveIncreases => _annualLeaveIncreaseRepository = _annualLeaveIncreaseRepository ?? new AnnualLeaveIncreaseRepository(_context, _identityContext);

        public IAppRoleRepository AppRoles => _appRoleRepository = _appRoleRepository ?? new AppRoleRepository(_context, _identityContext);

        public IAppUserRepository AppUsers => _appUserRepository = _appUserRepository ?? new AppUserRepository(_context, _identityContext);

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