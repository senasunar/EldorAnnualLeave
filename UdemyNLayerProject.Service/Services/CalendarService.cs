using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Repositories;
using EldorAnnualLeave.Core.Services;
using EldorAnnualLeave.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Service.Services
{
    public class CalendarService : Service<Calendar>, ICalendarService
    {
        private readonly IRepository<Calendar> _calendarRepository;

        public CalendarService(
            IUnitOfWork unitOfWork, 
            IRepository<Calendar> calendarRepository) : base(unitOfWork, calendarRepository)
        {
            _calendarRepository = calendarRepository;
        }

        public async Task<List<Calendar>> CreateEmployeeCalendarTable(string employeeID)
        {
            var getAllCalendar = await _calendarRepository.GetAllAsync();
            List<Calendar> requestedCalendar = new List<Calendar>();

            foreach (var calendar in getAllCalendar)
            {
                int calEmployeeID = int.Parse(calendar.Employee_ID);
                int parameterEmployeeID = int.Parse(employeeID);
                if (calEmployeeID == parameterEmployeeID) requestedCalendar.Add(calendar);
            }

            return requestedCalendar;
        }
    }
}
