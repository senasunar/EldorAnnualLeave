﻿using EldorAnnualLeave.Core.Models;
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
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Calendar> _calendarRepository;

        public CalendarService(
            IUnitOfWork unitOfWork, 
            IRepository<Calendar> calendarRepository, 
            IRepository<Employee> employeeRepository) : base(unitOfWork, calendarRepository)
        {
            _calendarRepository = calendarRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<List<Calendar>> CreateEmployeeCalendarTable(int employeeID)
        {
            var getAllCalendar = await _calendarRepository.GetAllAsync();
            List<Calendar> requestedCalendar = new List<Calendar>();

            foreach (var calendar in getAllCalendar)
            {
                if (calendar.Employee_ID == employeeID) requestedCalendar.Add(calendar);
            }

            return requestedCalendar;
        }
    }
}
