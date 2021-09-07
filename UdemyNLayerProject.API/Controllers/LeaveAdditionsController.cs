using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EldorAnnualLeave.API.DTOs;
using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EldorAnnualLeave.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeaveAdditionsController : ControllerBase
    {
        private readonly IAnnualLeaveIncreaseService _leaveAdditionService;
        private readonly IMapper _mapper;

        public LeaveAdditionsController(IAnnualLeaveIncreaseService leaveAdditionService, IMapper mapper)
        {
            _leaveAdditionService = leaveAdditionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var annualLeaveTypes = await _leaveAdditionService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<LeaveAdditionDto>>(annualLeaveTypes));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var leaveAddition = await _leaveAdditionService.GetByIdAsync(id);
            return Ok(_mapper.Map<LeaveAdditionDto>(leaveAddition));
        }

        [HttpPost]
        public async Task<IActionResult> Save(LeaveAdditionDto leaveAdditionDto)
        {
            var newLeaveAddition = await _leaveAdditionService.AddAsync(_mapper.Map<AnnualLeaveIncrease>(leaveAdditionDto));
            return Created(string.Empty, _mapper.Map<LeaveAdditionDto>(newLeaveAddition));
        }

        [HttpPut]
        public IActionResult Update(LeaveAdditionDto leaveAdditionDto)

        {
            var category = _leaveAdditionService.Update(_mapper.Map<AnnualLeaveIncrease>(leaveAdditionDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var leaveAddition = _leaveAdditionService.GetByIdAsync(id).Result;
            _leaveAdditionService.Remove(leaveAddition);

            return NoContent();
        }
    }
}
