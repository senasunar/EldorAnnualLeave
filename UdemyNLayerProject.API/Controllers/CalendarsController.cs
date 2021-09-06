using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EldorAnnualLeave.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EldorAnnualLeave.Core.Models;

namespace EldorAnnualLeave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarsController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarsController(ICalendarService service)
        {
            _calendarService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var calendars = await _calendarService.GetAllAsync();
            return Ok(calendars);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Calendar calendar)

        {
            var newCalendar = await _calendarService.AddAsync(calendar);

            return Ok(newCalendar);
        }
    }
}