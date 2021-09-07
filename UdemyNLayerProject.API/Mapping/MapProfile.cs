using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EldorAnnualLeave.API.DTOs;
using EldorAnnualLeave.Core.Models;

namespace EldorAnnualLeave.API.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<AnnualLeaveType, AnnualLeaveTypeDto>();
            CreateMap<AnnualLeaveTypeDto, AnnualLeaveType>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<AnnualLeaveIncrease, LeaveAdditionDto>();
            CreateMap<LeaveAdditionDto, AnnualLeaveIncrease>();

            CreateMap<Calendar, CalendarDto>();
            CreateMap<CalendarDto, Calendar>();

            CreateMap<AnnualLeaveIncrease, AnnualLeaveIncreaseDto>();
            CreateMap<AnnualLeaveIncreaseDto, AnnualLeaveIncrease>();
        }
    }
}