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
    public class AnnualLeaveIncreasesController : ControllerBase
    {
        private readonly IAnnualLeaveIncreaseService _annualLeaveIncreaseService;
        private readonly IMapper _mapper;

        public AnnualLeaveIncreasesController(IAnnualLeaveIncreaseService annualLeaveIncreaseService, IMapper mapper)
        {
            _annualLeaveIncreaseService = annualLeaveIncreaseService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()

        {
            var annualLeaveIncreases = await _annualLeaveIncreaseService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<AnnualLeaveIncreaseDto>>(annualLeaveIncreases));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _annualLeaveIncreaseService.GetByIdAsync(id);
            return Ok(_mapper.Map<AnnualLeaveIncreaseDto>(category));
        }

        [HttpPost]
        public async Task<IActionResult> Save(AnnualLeaveIncreaseDto annualLeaveIncreaseDto)
        {
            var newCategory = await _annualLeaveIncreaseService.AddAsync(_mapper.Map<AnnualLeaveIncrease>(annualLeaveIncreaseDto));
            return Created(string.Empty, _mapper.Map<AnnualLeaveIncreaseDto>(newCategory));
        }

        [HttpPut]
        public IActionResult Update(AnnualLeaveIncreaseDto annualLeaveIncreaseDto)

        {
            var category = _annualLeaveIncreaseService.Update(_mapper.Map<AnnualLeaveIncrease>(annualLeaveIncreaseDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var category = _annualLeaveIncreaseService.GetByIdAsync(id).Result;
            _annualLeaveIncreaseService.Remove(category);

            return NoContent();
        }
    }
}