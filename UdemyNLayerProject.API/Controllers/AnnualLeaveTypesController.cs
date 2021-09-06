using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EldorAnnualLeave.API.DTOs;
using EldorAnnualLeave.Core.Models;
using EldorAnnualLeave.Core.Services;

namespace EldorAnnualLeave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnualLeaveTypesController : ControllerBase
    {
        private readonly IAnnualLeaveTypeService _annualLeaveTypeService;
        private readonly IMapper _mapper;

        public AnnualLeaveTypesController(IAnnualLeaveTypeService annualLeaveTypeService, IMapper mapper)
        {
            _annualLeaveTypeService = annualLeaveTypeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()

        {
            var annualLeaveTypes = await _annualLeaveTypeService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<AnnualLeaveTypeDto>>(annualLeaveTypes));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _annualLeaveTypeService.GetByIdAsync(id);
            return Ok(_mapper.Map<AnnualLeaveTypeDto>(category));
        }

        [HttpPost]
        public async Task<IActionResult> Save(AnnualLeaveTypeDto annualLeaveTypeDto)
        {
            var newCategory = await _annualLeaveTypeService.AddAsync(_mapper.Map<AnnualLeaveType>(annualLeaveTypeDto));
            return Created(string.Empty, _mapper.Map<AnnualLeaveTypeDto>(newCategory));
        }

        [HttpPut]
        public IActionResult Update(AnnualLeaveTypeDto annualLeaveTypeDto)

        {
            var category = _annualLeaveTypeService.Update(_mapper.Map<AnnualLeaveType>(annualLeaveTypeDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var category = _annualLeaveTypeService.GetByIdAsync(id).Result;
            _annualLeaveTypeService.Remove(category);

            return NoContent();
        }
    }
}