using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ViewModels;
using AutoMapper;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{ 
    [ApiController]
    [Route("api/periodic-reports")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeriodicReportController : Controller
    {
        private readonly IPeriodicReportUseCases _periodicReportUseCases;
        private readonly IMapper _mapper;

        public PeriodicReportController(IPeriodicReportUseCases periodicReportUseCases, IMapper mapper)
        {
            _periodicReportUseCases = periodicReportUseCases;
            _mapper = mapper;
        }

        [HttpGet("{year}")]
        public async Task<IActionResult> GetByYearAndMonth(int year)
        {
            try
            {
                IEnumerable<PeriodicReportDto> periodicReportDto = await _periodicReportUseCases.GetByYearAndMonth(year);
                IEnumerable<PeriodicReportViewModel> periodicReportVM = _mapper.Map<IEnumerable<PeriodicReportDto>, IEnumerable<PeriodicReportViewModel>>(periodicReportDto);
                return Ok(periodicReportVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
