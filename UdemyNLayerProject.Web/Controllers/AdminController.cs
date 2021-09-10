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
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAnnualLeaveTypeService _annualLeaveTypeService;
        private readonly ICalendarService _calendarService;
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleService;
        private readonly IMapper _mapper;

        public AdminController(
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
            _appUserService = appUserService;
            _appRoleService = appRoleService;
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
            if (ReturnUrl.ToLower().Contains("manager"))
            {
                ViewBag.message = "This page is authorized for managers!";
            }
            else if (ReturnUrl.ToLower().Contains("member"))
            {
                ViewBag.message = "This page is authorized for members!";
            }
            else
            {
                ViewBag.message = "You are not authorized to view this page!";
            }

            return View();
        }

        public async Task<IActionResult> EmployeeTable()
        {
            var employeeTable = await _appUserService.CreateEmployeeTable();
            List<EmployeeTableViewModel> employeeTableModel = new List<EmployeeTableViewModel>();

            foreach (var employee in employeeTable)
            {
                if (employee.Is_Active == 1 && employee.Is_Deleted == 0)
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

        public async Task<IActionResult> AdminEnterLeave()
        {
            var employees = await _appUserService.GetAllUsersAsync();
            List<SelectListItem> employeeList = new List<SelectListItem>();

            foreach (var employee in employees)
            {
                if (employee.Is_Deleted == 0 && employee.Is_Active == 1)
                {
                    string nameSurname = employee.Employee_Name + " " + employee.Employee_Surname;
                    employeeList.Add(new SelectListItem
                    {
                        Text = nameSurname,
                        Value = employee.Email.ToString()
                    });
                }
            }

            ViewBag.employees = employeeList;
            if (TempData["error"] != null) ViewBag.error = TempData["error"].ToString();

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

        public bool IsClashed(DateTime startDate, DateTime endDate, AppUser employee)
        {
            bool isClashed = false;

            foreach(var calendar in employee.Calendar) 
            {
                if(calendar.Start_Day <= startDate && startDate <= calendar.End_Day)
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
        public async Task<IActionResult> AdminInsertLeave(AdminEnterLeaveViewModel enter)
        {
            var employeeTable = await _appUserService.CreateEmployeeTable();
            string eID = "";
            int isAllowed = 0;
            int dateValidator = 0;
            int isClashed = 0;
            //var employees = await _employeeService.GetAllAsync();

            Calendar calendar = new Calendar();

            foreach (var employee in employeeTable)
            {
                if (employee.Email == enter.Employeee)
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
                return RedirectToAction("AdminEnterLeave");
            }

            else if (dateValidator == 0)
            {
                TempData["error"] = "Start date cannot be later than end date!";
                return RedirectToAction("AdminEnterLeave");
            }

            else if (isClashed == 1)
            {
                TempData["error"] = "Date Clash!";
                return RedirectToAction("AdminEnterLeave");
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
                if(employee.Is_Deleted == 0)
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

        public async Task<IActionResult> AddUser()
        {
            var roles = await _appRoleService.GetAllRolesAsync();

            List<SelectListItem> User_Role = new List<SelectListItem>();

            foreach (var role in roles)
            {
                User_Role.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Id
                });
            }

            ViewBag.User_Role = User_Role;
            if (TempData["error"] != null) ViewBag.error = TempData["error"].ToString();
            
            return View();
        }

        public async Task<int> IDgeneratorAsync()
        {
            int checkID = 1;
            int userID;

            var users = await _appUserService.GetAllUsersAsync();

            foreach(var user in users)
            {
                userID = int.Parse(user.Id);

                if (userID != checkID)
                {
                    break;
                }

                checkID++;
            }

            return checkID;
        }

        [HttpPost]
        public async Task<IActionResult> InsertUser(UserViewModel user)
        {
            var usersList = await _appUserService.GetAllUsersAsync();

            foreach(var item in usersList)
            {
                if(String.Compare(item.Email, user.Email) == 0)
                {
                    TempData["error"] = "This email address is already taken!";
                    return RedirectToAction("AddUser");
                }

                if(String.Compare(item.UserName, user.UserName) == 0)
                {
                    TempData["error"] = "This username is already taken!";
                    return RedirectToAction("AddUser");
                }
            }
            
            AppUser appUser = new AppUser();

            //appUser.ID = "3";
            var passwordHash = userManager.PasswordHasher.HashPassword(appUser, user.Password);
            int generatedID = await IDgeneratorAsync();

            appUser.Id = generatedID.ToString();
            appUser.Email = user.Email;
            appUser.PasswordHash = passwordHash;
            appUser.Employee_Name = user.Employee_Name;
            appUser.Employee_Surname = user.Employee_Surname;
            appUser.UserName = user.UserName;
            appUser.Employee_ID = user.Employee_ID;
            appUser.SAP_ID = user.SAP_ID;
            appUser.Is_Active = 1;
            appUser.Is_Deleted = 0;
            appUser.AccessFailedCount = 0;
            appUser.EmailConfirmed = true;
            appUser.LockoutEnabled = true;
            appUser.NormalizedEmail = user.Email.ToUpper();
            appUser.NormalizedUserName = user.UserName.ToUpper();
            appUser.PhoneNumber = "";
            appUser.PhoneNumberConfirmed = true;
            appUser.TwoFactorEnabled = false;
            appUser.Entry_Date = user.Entry_Date;

            await _appUserService.AddUserAsync(appUser);

            var roles = await _appRoleService.GetAllRolesAsync();

            foreach(var role in roles)
            {
                int roleID = int.Parse(user.User_Role);
                int formRoleID = int.Parse(role.Id);

                if(roleID == formRoleID)
                {
                    await userManager.AddToRoleAsync(appUser, role.Name);
                    break;
                }
            }

            return RedirectToAction("EmployeeTable");
        }

        public async Task<IActionResult> UpdateUserAll()
        {
            var users = await _appUserService.GetAllUsersAsync();

            List<SelectListItem> User_List = new List<SelectListItem>();

            foreach (var user in users)
            {
                string nameSurname = user.Employee_Name + " " + user.Employee_Surname;

                User_List.Add(new SelectListItem
                {
                    Text = nameSurname,
                    Value = user.Id
                });
            }

            ViewBag.User_List = User_List;
            if (TempData["inform"] != null) ViewBag.inform = TempData["inform"].ToString();

            return View();
        }

        public async Task<IActionResult> UpdateUserPersonal(UpdateAllViewModel updateAll)
        {
            var user = await _appUserService.GetByUserIdAsync(updateAll.Id);
            ViewBag.user = user;

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

            return RedirectToAction("UpdateUserAll");
        }

        public async Task<IActionResult> UpdateUserRole(UpdateAllViewModel updateAll)
        {
            var roles = await _appRoleService.GetAllRolesAsync();
            var user = await _appUserService.GetByUserIdAsync(updateAll.Id);

            List<SelectListItem> User_Role = new List<SelectListItem>();

            foreach (var role in roles)
            {
                User_Role.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Id
                });
            }

            ViewBag.User_Role = User_Role;
            ViewBag.user = user;

            return View();
        }

        public async Task<IActionResult> UpdateUserRoleDB(UpdateRoleViewModel updateRole)
        {
            var user = await _appUserService.GetByUserIdAsync(updateRole.Id);

            var roles = await _appRoleService.GetAllRolesAsync();
            var currentRoles = await userManager.GetRolesAsync(user);

            foreach (var role in currentRoles)
            {
                await userManager.RemoveFromRoleAsync(user, role);
            }

            foreach (var role in roles)
            {
                int roleID = int.Parse(updateRole.User_Role);
                int formRoleID = int.Parse(role.Id);

                if (roleID == formRoleID)
                {
                    await userManager.AddToRoleAsync(user, role.Name);
                    break;
                }
            }

            TempData["inform"] = "User role is changed!";

            return RedirectToAction("UpdateUserAll");
        }

        public async Task<IActionResult> UpdateUserPassword(UpdateAllViewModel updateAll)
        {
            var user = await _appUserService.GetByUserIdAsync(updateAll.Id);
            
            ViewBag.user = user;
            if (TempData["passwordError"] != null) ViewBag.error = TempData["passwordError"].ToString();

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
                UpdateAllViewModel updateAll = new UpdateAllViewModel();
                updateAll.Id = updatePassword.Id;

                return RedirectToAction("UpdateUserPassword", updateAll);
            }

            if (String.Compare(updatePassword.Password, updatePassword.PasswordAgain) != 0)
            {
                TempData["passwordError"] = "New passwords are not matched!";
                UpdateAllViewModel updateAll = new UpdateAllViewModel();
                updateAll.Id = updatePassword.Id;

                return RedirectToAction("UpdateUserPassword", updateAll);
            }

            if (newPasswordCheck)
            {
                TempData["passwordError"] = "New password cannot be the same with the previous password!";
                UpdateAllViewModel updateAll = new UpdateAllViewModel();
                updateAll.Id = updatePassword.Id;

                return RedirectToAction("UpdateUserPassword", updateAll);
            }

            IdentityResult result = userManager.ChangePasswordAsync(user, updatePassword.CurrentPassword, updatePassword.Password).Result;

            if(result.Succeeded) TempData["inform"] = "Password is changed!";
            else TempData["inform"] = "Failed! Please follow the password rules!"; //needs spesification

            return RedirectToAction("UpdateUserAll");
        }

        public async Task<IActionResult> UserOperation()
        {
            var users = await _appUserService.GetAllUsersAsync();

            List<SelectListItem> User_List = new List<SelectListItem>();

            foreach (var user in users)
            {
                string nameSurname = user.Employee_Name + " " + user.Employee_Surname;

                User_List.Add(new SelectListItem
                {
                    Text = nameSurname,
                    Value = user.Id
                });
            }

            ViewBag.User_List = User_List;
            if (TempData["inform"] != null) ViewBag.error = TempData["inform"].ToString();

            return View();
        }

        public async Task<IActionResult> DeleteUser(UserOperationViewModel userOperation)
        {
            var user = await _appUserService.GetByUserIdAsync(userOperation.Id);
            user.Is_Deleted = 1;

            var updatedUser = _appUserService.UpdateUser(user);

            TempData["inform"] = "User is deleted!";

            return RedirectToAction("UserOperation");
        }

        public async Task<IActionResult> InactiveUser(UserOperationViewModel userOperation)
        {
            var user = await _appUserService.GetByUserIdAsync(userOperation.Id);
            user.Is_Active = 0;
            
            var updatedUser = _appUserService.UpdateUser(user);

            TempData["inform"] = "User is inactivated!";

            return RedirectToAction("UserOperation");
        }
    }
}
