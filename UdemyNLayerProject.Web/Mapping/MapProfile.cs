using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Web.DTOs;

namespace EldorAnnualLeave.Web.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<AnnualLeaveType, AnnualLeaveTypeDto>();
            CreateMap<AnnualLeaveTypeDto, AnnualLeaveType>();

            CreateMap<AnnualLeaveIncrease, LeaveAdditionDto>();
            CreateMap<LeaveAdditionDto, AnnualLeaveIncrease>();

            CreateMap<Calendar, CalendarDto>();
            CreateMap<CalendarDto, Calendar>();

            CreateMap<AnnualLeaveIncrease, AnnualLeaveIncreaseDto>();
            CreateMap<AnnualLeaveIncreaseDto, AnnualLeaveIncrease>();
        }
    }
}