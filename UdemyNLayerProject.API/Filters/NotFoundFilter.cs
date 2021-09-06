using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EldorAnnualLeave.API.DTOs;
using EldorAnnualLeave.Core.Services;

namespace EldorAnnualLeave.API.Filters
{
    public class NotFoundFilter : ActionFilterAttribute
    {
        private readonly IEmployeeService _employeeService;

        public NotFoundFilter(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int id = (int)context.ActionArguments.Values.FirstOrDefault();

            var employee = await _employeeService.GetByIdAsync(id);

            if (employee != null)
            {
                await next();
            }
            else
            {
                ErrorDto errorDto = new ErrorDto();

                errorDto.Status = 404;

                errorDto.Errors.Add($"id'si {id} olan ürün veritabanında bulunamadı");

                context.Result = new NotFoundObjectResult(errorDto);
            }
        }
    }
}