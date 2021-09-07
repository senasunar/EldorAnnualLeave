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
    public class AppUserService : Service<AppUser>, IAppUserService
    {
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly IRepository<Calendar> _calendarRepository;
        private readonly IRepository<AnnualLeaveIncrease> _annualLeaveIncreaseRepository;

        public AppUserService(
            IUnitOfWork unitOfWork, 
            IRepository<AppUser> repository,
            IRepository<Calendar> calendarRepository,
            IRepository<AnnualLeaveIncrease> annualLeaveIncreaseRepository) : base(unitOfWork, repository)
        {
            _appUserRepository = repository;
            _calendarRepository = calendarRepository;
            _annualLeaveIncreaseRepository = annualLeaveIncreaseRepository;
        }

        public async Task AddUserAsync(AppUser appUser)
        {
            await _appUserRepository.AddUserAsync(appUser);
            await _unitOfWork.CommitAsyncIdentity();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _appUserRepository.GetAllUsersAsync();
        }

        public Task<AppUser> GetByUserIdAsync(string id)
        {
            return _appUserRepository.GetByUserIdAsync(id);
        }

        public void RemoveUser(AppUser appUser)
        {
            _appUserRepository.RemoveUser(appUser);
        }

        public AppUser UpdateUser(AppUser appUser)
        {
            AppUser updatedUser = _appUserRepository.UpdateUser(appUser);
            _unitOfWork.CommitIdentity();
            return updatedUser;
        }

        public async Task<List<AppUser>> CreateEmployeeTable()
        {
            var getAllUsers = await _appUserRepository.GetAllUsersAsync();
            var getAllCalendar = await _calendarRepository.GetAllAsync();
            var getAllIncrease = await _annualLeaveIncreaseRepository.GetAllAsync();

            List<Calendar> existCalendars = new List<Calendar>();

            foreach (var cal in getAllCalendar)
            {
                if (cal.Is_Deleted == 0) existCalendars.Add(cal);
            }

            List<AppUser> joined = (from user in getAllUsers.ToList()
                                     join calen in existCalendars on int.Parse(user.Id) equals int.Parse(calen.Employee_ID) into c
                                     select new AppUser()
                                     {
                                         Email = user.Email,
                                         annualLeave = user.annualLeave,
                                         Employee_ID = user.Employee_ID,
                                         Employee_Name = user.Employee_Name,
                                         PasswordHash = user.PasswordHash,
                                         Employee_Surname = user.Employee_Surname,
                                         Entry_Date = user.Entry_Date,
                                         Is_Active = user.Is_Active,
                                         Is_Deleted = user.Is_Deleted,
                                         Id = user.Id,
                                         SAP_ID = user.SAP_ID,
                                         Calendar = c.ToList()
                                     }
                        ).ToList();

            foreach (var employee in joined)
            {
                int usedLeave = 0;
                int plannedLeave = 0;
                int leavePeriod = 0;
                int annualLeave = 0;
                int temp = 0;

                TimeSpan ts = DateTime.Now.Subtract(employee.Entry_Date);
                int years = ((int)ts.TotalDays) / 365;

                for (int i = 0; i < (getAllIncrease.ToList().Count); i++)
                {
                    if (getAllIncrease.ToList().ElementAt(i).Completed_Year <= years && i == (getAllIncrease.ToList().Count - 1))
                    {
                        annualLeave += getAllIncrease.ToList().ElementAt(i).Day_Addition * (years - getAllIncrease.ToList().ElementAt(i).Completed_Year);
                        break;
                    }

                    if (getAllIncrease.ToList().ElementAt(i).Completed_Year <= years)
                    {
                        temp = (years > getAllIncrease.ToList().ElementAt(i + 1).Completed_Year) ? getAllIncrease.ToList().ElementAt(i + 1).Completed_Year : years;
                        annualLeave += getAllIncrease.ToList().ElementAt(i).Day_Addition * (temp - getAllIncrease.ToList().ElementAt(i).Completed_Year);
                    }

                    if (getAllIncrease.ToList().ElementAt(i + 1).Completed_Year > years)
                    {
                        annualLeave += getAllIncrease.ToList().ElementAt(i).Day_Addition;
                        break;
                    }
                }

                foreach (var leaveDate in employee.Calendar)
                {
                    TimeSpan ts1 = DateTime.Now.Subtract(leaveDate.End_Day);
                    TimeSpan ts2 = leaveDate.End_Day.Subtract(leaveDate.Start_Day);
                    leavePeriod = ((int)ts2.TotalDays);

                    if (ts1.TotalDays < 0) plannedLeave += leavePeriod;
                    else if (ts1.TotalDays >= 0) usedLeave += leavePeriod;
                }

                employee.annualLeave = annualLeave;
                employee.usedLeave = usedLeave;
                employee.plannedLeave = plannedLeave;
                employee.totalLeave = employee.usedLeave + employee.plannedLeave;
                employee.restOfLeave = annualLeave - employee.usedLeave - employee.plannedLeave;
            }

            return joined;
        }

        public async Task<List<AppUser>> CreateEmployeeTableMember(string email)
        {
            var getAllEmployee = await _appUserRepository.GetAllUsersAsync();
            var getAllCalendar = await _calendarRepository.GetAllAsync();
            var getAllIncrease = await _annualLeaveIncreaseRepository.GetAllAsync();

            List<AppUser> joined = (from emp in getAllEmployee.ToList()
                                     join calen in getAllCalendar.ToList() on int.Parse(emp.Id) equals int.Parse(calen.Employee_ID) into c
                                     select new AppUser()
                                     {
                                         Email = emp.Email,
                                         //Annual_Leave = emp.Annual_Leave,
                                         Employee_ID = emp.Employee_ID,
                                         Employee_Name = emp.Employee_Name,
                                         PasswordHash = emp.PasswordHash,
                                         Employee_Surname = emp.Employee_Surname,
                                         Entry_Date = emp.Entry_Date,
                                         Is_Active = emp.Is_Active,
                                         Is_Deleted = emp.Is_Deleted,
                                         Id = emp.Id,
                                         SAP_ID = emp.SAP_ID,
                                         Calendar = c.ToList()
                                     }
                        ).ToList();

            List<AppUser> result = new List<AppUser>();

            foreach (var employee in joined)
            {
                if (employee.Email == email)
                {
                    int usedLeave = 0;
                    int plannedLeave = 0;
                    int leavePeriod = 0;
                    int annualLeave = 0;
                    int temp = 0;

                    TimeSpan ts = DateTime.Now.Subtract(employee.Entry_Date);
                    int years = ((int)ts.TotalDays) / 365;

                    for (int i = 0; i < (getAllIncrease.ToList().Count); i++)
                    {
                        if (getAllIncrease.ToList().ElementAt(i).Completed_Year <= years && i == (getAllIncrease.ToList().Count - 1))
                        {
                            annualLeave += getAllIncrease.ToList().ElementAt(i).Day_Addition * (years - getAllIncrease.ToList().ElementAt(i).Completed_Year);
                            break;
                        }

                        if (getAllIncrease.ToList().ElementAt(i).Completed_Year <= years)
                        {
                            temp = (years > getAllIncrease.ToList().ElementAt(i + 1).Completed_Year) ? getAllIncrease.ToList().ElementAt(i + 1).Completed_Year : years;
                            annualLeave += getAllIncrease.ToList().ElementAt(i).Day_Addition * (temp - getAllIncrease.ToList().ElementAt(i).Completed_Year);
                        }

                        if (getAllIncrease.ToList().ElementAt(i + 1).Completed_Year > years)
                        {
                            annualLeave += getAllIncrease.ToList().ElementAt(i).Day_Addition;
                            break;
                        }
                    }

                    foreach (var leaveDate in employee.Calendar)
                    {
                        TimeSpan ts1 = DateTime.Now.Subtract(leaveDate.End_Day);
                        TimeSpan ts2 = leaveDate.End_Day.Subtract(leaveDate.Start_Day);
                        leavePeriod = ((int)ts2.TotalDays);

                        if (ts1.TotalDays < 0) plannedLeave += leavePeriod;
                        else if (ts1.TotalDays >= 0) usedLeave += leavePeriod;
                    }

                    employee.usedLeave = usedLeave;
                    employee.plannedLeave = plannedLeave;
                    employee.totalLeave = employee.usedLeave + employee.plannedLeave;
                    employee.restOfLeave = annualLeave - employee.usedLeave - employee.plannedLeave;

                    result.Add(employee);

                    break;
                }
            }

            return result;
        }
    }
}
