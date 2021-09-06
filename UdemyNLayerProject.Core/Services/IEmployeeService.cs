using EldorAnnualLeave.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Core.Services
{
    public interface IEmployeeService : IService<Employee>
    {
        public Task<List<Employee>> CreateEmployeeTable();
        public Task<List<Employee>> CreateEmployeeTableMember(string email);
    }
}
