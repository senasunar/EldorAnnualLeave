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
    public class EmployeeService : Service<Employee>, IEmployeeService
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly ICalendarRepository _calendarRepository;

        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Calendar> _calendarRepository;
        private readonly IRepository<AnnualLeaveIncrease> _annualLeaveIncreaseRepository;

        public EmployeeService(
            IUnitOfWork unitOfWork,
            IRepository<Employee> employeeRepository,
            IRepository<Calendar> calendarRepository,
            IRepository<AnnualLeaveIncrease> annualLeaveIncreaseRepository) : base(unitOfWork, employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _calendarRepository = calendarRepository;
            _annualLeaveIncreaseRepository = annualLeaveIncreaseRepository;
        }

        public async Task<List<Employee>> CreateEmployeeTable()
        {
            var getAllEmployee = await _employeeRepository.GetAllAsync();
            var getAllCalendar = await _calendarRepository.GetAllAsync();
            var getAllIncrease = await _annualLeaveIncreaseRepository.GetAllAsync();

            List<Calendar> existCalendars = new List<Calendar>();

            foreach(var cal in getAllCalendar)
            {
                if (cal.Is_Deleted == 0) existCalendars.Add(cal);
            }

            List<Employee> joined = (from emp in getAllEmployee.ToList()
                        join calen in existCalendars on int.Parse(emp.ID) equals int.Parse(calen.Employee_ID) into c
                        select new Employee()
                        {
                            Email = emp.Email,
                            annualLeave = emp.annualLeave,
                            Employee_ID = emp.Employee_ID,
                            Employee_Name = emp.Employee_Name,
                            PasswordHash = emp.PasswordHash,
                            Employee_Surname = emp.Employee_Surname,
                            Entry_Date = emp.Entry_Date,
                            Is_Active = emp.Is_Active,
                            Is_Deleted = emp.Is_Deleted,
                            ID = emp.ID,
                            SAP_ID = emp.SAP_ID,
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

        public async Task<List<Employee>> CreateEmployeeTableMember(string email)
        {
            var getAllEmployee = await _employeeRepository.GetAllAsync();
            var getAllCalendar = await _calendarRepository.GetAllAsync();
            var getAllIncrease = await _annualLeaveIncreaseRepository.GetAllAsync();

            List<Employee> joined = (from emp in getAllEmployee.ToList()
                                     join calen in getAllCalendar.ToList() on int.Parse(emp.ID) equals int.Parse(calen.Employee_ID) into c
                                     select new Employee()
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
                                         ID = emp.ID,
                                         SAP_ID = emp.SAP_ID,
                                         Calendar = c.ToList()
                                     }
                        ).ToList();

            List<Employee> result = new List<Employee>();

            foreach (var employee in joined)
            {
                if(employee.Email == email)
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
