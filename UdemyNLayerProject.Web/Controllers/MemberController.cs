using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//using EldorAnnualLeave.Enums;
using EldorAnnualLeave.Web.ViewModels;
using EldorAnnualLeave.Web.Controllers;
using System.Collections.Generic;
using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Services;
using EldorAnnualLeave.Web.DTOs;
using EldorAnnualLeave.Web.Filters;
using AutoMapper;
using EldorAnnualLeave.Service.Services;
using System.Data;

namespace EldorAnnualLeave.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAnnualLeaveTypeService _annualLeaveTypeService;
        private readonly ICalendarService _calendarService;
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleService;
        private readonly IMapper _mapper;

        public MemberController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            IEmployeeService employeeService, 
            IAnnualLeaveTypeService annualLeaveTypeService, 
            ICalendarService calendarService,
            IAppUserService appUserService,
            IAppRoleService appRoleService,
            IMapper mapper) : base(userManager, signInManager)
        {
            _employeeService = employeeService;
            _annualLeaveTypeService = annualLeaveTypeService;
            _calendarService = calendarService;
            _appUserService = appUserService;
            _appRoleService = appRoleService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Eldor");
            return RedirectToAction("LoginPage", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> EmployeeTable()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var employeeTable =  await _appUserService.CreateEmployeeTableMember(email);
            List<EmployeeTableViewModel> employeeTableModel = new List<EmployeeTableViewModel>();

            foreach (var employee in employeeTable)
            {
                if(employee.Is_Active == 1 && employee.Is_Deleted == 0)
                {
                    EmployeeTableViewModel etm = new EmployeeTableViewModel();
                    etm.Employee_ID = employee.Id;
                    etm.Employee_Name = employee.Employee_Name;
                    etm.Employee_Surname = employee.Employee_Surname;
                    etm.Entry_Date = employee.Entry_Date;
                    etm.Planned_Leave = employee.plannedLeave;
                    etm.Rest_Of_Leave = employee.restOfLeave;
                    etm.Used_Leave = employee.usedLeave;
                    etm.Total_Leave = employee.totalLeave;
                    etm.Annual_Leave = employee.annualLeave;

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
                if(leave.Is_Deleted == 0 && leave.Is_Active == 1)
                {
                    ddleaves.Add(new SelectListItem
                    {
                        Text = leave.ALT_Name,
                        Value = leave.ID.ToString()
                    });
                }
            }

            ViewBag.annualLeaveTypeList = ddleaves;
            if(TempData["error"] != null) ViewBag.error = TempData["error"].ToString();
            return View();
        }

        public bool IsClashed(DateTime startDate, DateTime endDate, AppUser employee)
        {
            bool isClashed = false;

            foreach (var calendar in employee.Calendar)
            {
                if (calendar.Start_Day <= startDate && startDate <= calendar.End_Day)
                {
                    isClashed = true;
                    break;
                }

                if (calendar.Start_Day <= endDate && endDate <= calendar.End_Day)
                {
                    isClashed = true;
                    break;
                }

                if (calendar.Start_Day >= startDate && endDate >= calendar.End_Day)
                {
                    isClashed = true;
                    break;
                }
            }
            return isClashed;
        }

        [HttpPost]
        public async Task<IActionResult> InsertLeave(EnterLeaveViewModel enter)
        {
            var employeeTable = await _appUserService.CreateEmployeeTable();
            var email = User.FindFirstValue(ClaimTypes.Email);
            string eID = "";
            int isAllowed = 0;
            int isClashed = 0;
            int dateValidator = 0;
            //var employees = await _employeeService.GetAllAsync();

            Calendar calendar = new Calendar();

            foreach (var employee in employeeTable)
            {
                if (employee.Email == email)
                {
                    eID = employee.Id;

                    TimeSpan ts = enter.End_Day.Subtract(enter.Start_Day);
                    int days = ((int)ts.TotalDays);

                    if (employee.restOfLeave >= days) isAllowed = 1;
                    if (days >= 0) dateValidator = 1;
                    if (IsClashed(enter.Start_Day, enter.End_Day, employee)) isClashed = 1;

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

            else if (isClashed == 1)
            {
                TempData["error"] = "Date Clash!";
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
                return RedirectToAction("EmployeeTable");
            }
        }

        public async Task<IActionResult> EmployeeCalendarTable(string employeeID)
        {
            var employeeCalendar = await _calendarService.CreateEmployeeCalendarTable(employeeID);

            List<CalendarDto> employeeCalendarTableModel = new List<CalendarDto>();

            foreach (var employee in employeeCalendar)
            {
                if (employee.Is_Deleted == 0)
                {
                    var annualLeaveType = await _annualLeaveTypeService.GetByIdAsync(employee.AnnualLeaveType_ID);

                    CalendarDto calendar = new CalendarDto();
                    calendar.Start_Day = employee.Start_Day;
                    calendar.End_Day = employee.End_Day;
                    calendar.annualLeaveType = annualLeaveType.ALT_Name;
                    calendar.ID = employee.ID;

                    TimeSpan ts = calendar.End_Day.Subtract(calendar.Start_Day);
                    calendar.total = ((int)ts.TotalDays);

                    employeeCalendarTableModel.Add(calendar);
                }
            }
            //var userID = signInManager
            return View(employeeCalendarTableModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCalendarRecord(int ID)
        {
            var calendar = await _calendarService.GetByIdAsync(ID);
            calendar.Is_Deleted = 1;
            var result = _calendarService.Update(calendar);

            return RedirectToAction("EmployeeTable");
        }

        public async Task<IActionResult> UpdateUserPersonal()
        {
            var users = await _appUserService.GetAllUsersAsync();
            var email = User.FindFirstValue(ClaimTypes.Email);
            string Id = "";

            foreach (var usr in users)
            {
                if (String.Compare(usr.Email, email) == 0)
                {
                    Id = usr.Id;
                    break;
                }
            }

            var user = await _appUserService.GetByUserIdAsync(Id);
            ViewBag.user = user;
            if (TempData["inform"] != null) ViewBag.inform = TempData["inform"].ToString();

            return View();
        }

        public async Task<IActionResult> UpdateUserPersonalDB(UpdatePersonalViewModel updatePersonal)
        {
            var user = await _appUserService.GetByUserIdAsync(updatePersonal.Id);
            user.Employee_Name = updatePersonal.Employee_Name;
            user.Employee_Surname = updatePersonal.Employee_Surname;
            user.UserName = updatePersonal.UserName;
            user.Email = updatePersonal.Email;

            var updatedUser = _appUserService.UpdateUser(user);

            TempData["inform"] = "Personal information is updated!";

            return RedirectToAction("UpdateUserPersonal");
        }

        public async Task<IActionResult> UpdateUserPassword()
        {
            var users = await _appUserService.GetAllUsersAsync();
            var email = User.FindFirstValue(ClaimTypes.Email);
            string Id = "";

            foreach (var usr in users)
            {
                if (String.Compare(usr.Email, email) == 0)
                {
                    Id = usr.Id;
                    break;
                }
            }

            ViewBag.Id = Id;
            if (TempData["passwordError"] != null) ViewBag.error = TempData["passwordError"].ToString();
            if (TempData["inform"] != null) ViewBag.inform = TempData["inform"].ToString();

            return View();
        }

        public async Task<IActionResult> UpdateUserPasswordDB(UpdatePasswordViewModel updatePassword)
        {
            var user = await _appUserService.GetByUserIdAsync(updatePassword.Id);
            var passwordCheck = await userManager.CheckPasswordAsync(user, updatePassword.CurrentPassword);
            var newPasswordCheck = await userManager.CheckPasswordAsync(user, updatePassword.Password);

            if (!passwordCheck)
            {
                TempData["passwordError"] = "Current password is wrong!";

                return RedirectToAction("UpdateUserPassword");
            }

            if (String.Compare(updatePassword.Password, updatePassword.PasswordAgain) != 0)
            {
                TempData["passwordError"] = "New passwords are not matched!";

                return RedirectToAction("UpdateUserPassword");
            }

            if (newPasswordCheck)
            {
                TempData["passwordError"] = "New password cannot be the same with the previous password!";

                return RedirectToAction("UpdateUserPassword");
            }

            IdentityResult result = userManager.ChangePasswordAsync(user, updatePassword.CurrentPassword, updatePassword.Password).Result;

            if (result.Succeeded) TempData["inform"] = "Password is changed!";
            else TempData["inform"] = "Failed! Please follow the password rules!"; //needs spesification

            return RedirectToAction("UpdateUserPassword");
        }

        public async Task<IActionResult> UpdateCalendar(int ID)
        {
            var calendar = await _calendarService.GetByIdAsync(ID);
            var annualLeaveType = await _annualLeaveTypeService.GetByIdAsync(calendar.AnnualLeaveType_ID);

            EnterLeaveViewModel enterLeave = new EnterLeaveViewModel();
            enterLeave.annualLeaveTypeList = annualLeaveType.ID.ToString();
            enterLeave.Start_Day = calendar.Start_Day;
            enterLeave.End_Day = calendar.End_Day;
            enterLeave.Id = calendar.Employee_ID;
            enterLeave.Calendar_ID = ID;

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

            if (TempData["error"] != null) ViewBag.error = TempData["error"].ToString();
            enterLeave.ALselectList = ddleaves;
            return View(enterLeave);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCalendarDB(EnterLeaveViewModel enter)
        {
            var employeeTable = await _appUserService.CreateEmployeeTable();
            string eID = "";
            int isAllowed = 0;
            int dateValidator = 0;
            int isClashed = 0;
            //var employees = await _employeeService.GetAllAsync();

            //Calendar calendar = new Calendar();
            var calendar = await _calendarService.GetByIdAsync(enter.Calendar_ID);

            foreach (var employee in employeeTable)
            {
                if (employee.Id == enter.Id)
                {
                    eID = employee.Id;

                    TimeSpan ts = enter.End_Day.Subtract(enter.Start_Day);
                    int days = ((int)ts.TotalDays);

                    if (employee.restOfLeave >= days) isAllowed = 1;
                    if (days >= 0) dateValidator = 1;
                    if (IsClashed(enter.Start_Day, enter.End_Day, employee)) isClashed = 1;

                    break;
                }
            }

            if (isAllowed == 0)
            {
                TempData["error"] = "Do not have enough leave!";
                return RedirectToAction("UpdateCalendar", new { ID = calendar.ID });
            }

            else if (dateValidator == 0)
            {
                TempData["error"] = "Start date cannot be later than end date!";
                return RedirectToAction("UpdateCalendar", new { ID = calendar.ID });
            }

            else if (isClashed == 1)
            {
                TempData["error"] = "Date Clash!";
                return RedirectToAction("UpdateCalendar", new { ID = calendar.ID });
            }

            else
            {
                calendar.Start_Day = enter.Start_Day;
                calendar.End_Day = enter.End_Day;
                calendar.AnnualLeaveType_ID = Int32.Parse(enter.annualLeaveTypeList);

                var result = _calendarService.Update(calendar);
                return RedirectToAction("EmployeeTable");
            }
        }
    }
}