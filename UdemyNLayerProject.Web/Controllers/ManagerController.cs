using AutoMapper;
using EldorAnnualLeave.Controllers;
using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Services;
using EldorAnnualLeave.Web.DTOs;
using EldorAnnualLeave.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAnnualLeaveTypeService _annualLeaveTypeService;
        private readonly ICalendarService _calendarService;
        private readonly IMapper _mapper;

        public ManagerController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmployeeService employeeService, IAnnualLeaveTypeService annualLeaveTypeService, ICalendarService calendarService, IMapper mapper) : base(userManager, signInManager)
        {
            _employeeService = employeeService;
            _annualLeaveTypeService = annualLeaveTypeService;
            _calendarService = calendarService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Eldor");
            return RedirectToAction("LoginPage", "Home");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            if (ReturnUrl.ToLower().Contains("violencegage"))
            {
                ViewBag.message = "Erişmeye çalıştığınız sayfa şiddet videoları içerdiğinden dolayı 15 yaşında büyük olmanız gerekmektedir";
            }
            else if (ReturnUrl.ToLower().Contains("ankarapage"))
            {
                ViewBag.message = "Bu sayfaya sadece şehir alanı ankara olan kullanıcılar erişebilir";
            }
            else if (ReturnUrl.ToLower().Contains("exchange"))
            {
                ViewBag.message = "30 günlük ücretsiz deneme hakkınız sona ermiştir.";
            }
            else
            {
                ViewBag.message = "Bu sayfaya erişim izniniz yoktur. Erişim izni almak için site yöneticisiyle görüşünüz";
            }

            return View();
        }

        public async Task<IActionResult> EmployeeTable()
        {
            var employeeTable = await _employeeService.CreateEmployeeTable();
            List<EmployeeTableViewModel> employeeTableModel = new List<EmployeeTableViewModel>();

            foreach (var employee in employeeTable)
            {
                if (employee.Is_Active == 1 && employee.Is_Deleted == 0)
                {
                    EmployeeTableViewModel etm = new EmployeeTableViewModel();
                    etm.Employee_ID = employee.ID;
                    etm.Employee_Name = employee.Employee_Name;
                    etm.Employee_Surname = employee.Employee_Surname;
                    etm.Entry_Date = employee.Entry_Date;
                    etm.Planned_Leave = employee.plannedLeave;
                    etm.Rest_Of_Leave = employee.restOfLeave;
                    etm.Used_Leave = employee.usedLeave;
                    etm.Total_Leave = employee.totalLeave;

                    employeeTableModel.Add(etm);
                }
            }

            //_mapper.Map<IEnumerable<Employee>>(employeeTable)

            return View(employeeTableModel);
        }

        public async Task<IActionResult> EnterLeave()
        {
            var leaves = await _annualLeaveTypeService.GetAllAsync();
            List<SelectListItem> ddleaves = new List<SelectListItem>();

            foreach (var leave in leaves)
            {
                if (leave.Is_Deleted == 0 && leave.Is_Active == 1)
                {
                    ddleaves.Add(new SelectListItem
                    {
                        Text = leave.ALT_Name,
                        Value = leave.ID.ToString()
                    });
                }
            }

            ViewBag.annualLeaveTypeList = ddleaves;
            if (TempData["error"] != null) ViewBag.error = TempData["error"].ToString();
            return View();
        }

        public bool IsClashed(DateTime startDate, DateTime endDate, Employee employee)
        {
            bool isClashed = false;



            return isClashed;
        }

        [HttpPost]
        public async Task<IActionResult> InsertLeave(EnterLeaveViewModel enter)
        {
            var employeeTable = await _employeeService.CreateEmployeeTable();
            var email = User.FindFirstValue(ClaimTypes.Email);
            string eID = "";
            int isAllowed = 0;
            int dateValidator = 0;
            //var employees = await _employeeService.GetAllAsync();

            Calendar calendar = new Calendar();

            foreach (var employee in employeeTable)
            {
                if (employee.Email == email)
                {
                    eID = employee.ID;

                    TimeSpan ts = enter.End_Day.Subtract(enter.Start_Day);
                    int days = ((int)ts.TotalDays);

                    if (employee.restOfLeave >= days) isAllowed = 1;
                    if (days >= 0) dateValidator = 1;

                    break;
                }
            }

            if (isAllowed == 0)
            {
                TempData["error"] = "You do not have enough leave!";
                return RedirectToAction("EnterLeave");
            }

            else if (dateValidator == 0)
            {
                TempData["error"] = "Start date cannot be later than end date!";
                return RedirectToAction("EnterLeave");
            }

            else
            {
                calendar.Employee_ID = eID;
                calendar.Is_Deleted = 0;
                calendar.Start_Day = enter.Start_Day;
                calendar.End_Day = enter.End_Day;
                calendar.AnnualLeaveType_ID = Int32.Parse(enter.annualLeaveTypeList);

                await _calendarService.AddAsync(_mapper.Map<Calendar>(calendar));
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> EmployeeCalendarTable(string employeeID)
        {
            var employeeCalendar = await _calendarService.CreateEmployeeCalendarTable(employeeID);

            List<CalendarDto> employeeCalendarTableModel = new List<CalendarDto>();

            foreach (var employee in employeeCalendar)
            {
                var annualLeaveType = await _annualLeaveTypeService.GetByIdAsync(employee.AnnualLeaveType_ID);

                CalendarDto calendar = new CalendarDto();
                calendar.Start_Day = employee.Start_Day;
                calendar.End_Day = employee.End_Day;
                calendar.annualLeaveType = annualLeaveType.ALT_Name;

                TimeSpan ts = calendar.End_Day.Subtract(calendar.Start_Day);
                calendar.total = ((int)ts.TotalDays);

                employeeCalendarTableModel.Add(calendar);
            }

            return View(employeeCalendarTableModel);
        }
    }
}
