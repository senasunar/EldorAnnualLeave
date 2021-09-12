using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Repositories;
using EldorAnnualLeave.Core.Services;
using EldorAnnualLeave.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EldorAnnualLeave.Service.Services
{
    public class AnnualLeaveTypeService : Service<AnnualLeaveType>, IAnnualLeaveTypeService
    {
        private readonly IRepository<AnnualLeaveType> _annualLeaveTypeRepository;

        public AnnualLeaveTypeService(
            IUnitOfWork unitOfWork, 
            IRepository<AnnualLeaveType> repository) : base(unitOfWork, repository)
        {
            _annualLeaveTypeRepository = repository;
        }

        public async Task<List<AnnualLeaveType>> CreateLeaveTypeTable()
        {
            var getAllLeaveTypes = await _annualLeaveTypeRepository.GetAllAsync();
            List<AnnualLeaveType> annualLeaveTypes = new List<AnnualLeaveType>();

            foreach(var leave in getAllLeaveTypes)
            {
                annualLeaveTypes.Add(leave);
            }

            return annualLeaveTypes;
        }
    }
}
