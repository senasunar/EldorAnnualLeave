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

        private AnnualLeaveTypeRepository _annualLeaveTypeRepository;
        private CalendarRepository _calendarRepository;
        private EmployeeRepository _employeeRepository;
        private LeaveAdditionRepository _leaveAdditionRepository;
        private AnnualLeaveIncreaseRepository _annualLeaveIncreaseRepository;

        public IAnnualLeaveTypeRepository AnnualLeaveTypes => _annualLeaveTypeRepository = _annualLeaveTypeRepository ?? new AnnualLeaveTypeRepository(_context);

        public ICalendarRepository Calendars => _calendarRepository = _calendarRepository ?? new CalendarRepository(_context);

        public IEmployeeRepository Employees => _employeeRepository = _employeeRepository ?? new EmployeeRepository(_context);

        public ILeaveAdditionRepository LeaveAdditions => _leaveAdditionRepository = _leaveAdditionRepository ?? new LeaveAdditionRepository(_context);

        public IAnnualLeaveIncreaseRepository AnnualLeaveIncreases => _annualLeaveIncreaseRepository = _annualLeaveIncreaseRepository ?? new AnnualLeaveIncreaseRepository(_context);

        public UnitOfWork(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}