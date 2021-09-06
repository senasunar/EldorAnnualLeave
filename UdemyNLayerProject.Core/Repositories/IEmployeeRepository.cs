using EldorAnnualLeave.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Core.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        void CalculateUsed(Employee employee);
        void CalculateRest(Employee employee);
        void CalculateTotal(Employee employee);
    }
}
