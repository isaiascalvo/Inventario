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
    [Route("api/reports")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReportController : Controller
    {
        private readonly IPeriodicReportUseCases _periodicReportUseCases;
        private readonly IMapper _mapper;

        public ReportController(IPeriodicReportUseCases periodicReportUseCases, IMapper mapper)
        {
            _periodicReportUseCases = periodicReportUseCases;
            _mapper = mapper;
        }

        [HttpGet("AnnualReport/{year}")]
        public async Task<IActionResult> GetAnnualReport(int year)
        {
            try
            {
                IEnumerable<PeriodicReportDto> annualReportDto = await _periodicReportUseCases.GetAnnualReport(year);
                IEnumerable<PeriodicReportViewModel> annualReportVM = _mapper.Map<IEnumerable<PeriodicReportDto>, IEnumerable<PeriodicReportViewModel>>(annualReportDto);
                return Ok(annualReportVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("FeesReport")]
        public async Task<IActionResult> GetFeesReport([FromQuery] FeesReportsFiltersViewModel filtersVM)
        {
            try
            {
                var filtersDto = _mapper.Map<FeesReportsFiltersViewModel, FeesReportsFiltersDto>(filtersVM);
                IEnumerable<FeesReportDto> feesReportDto = await _periodicReportUseCases.GetFeesReport(filtersDto);
                IEnumerable<FeesReportViewModel> feesReportVM = _mapper.Map<IEnumerable<FeesReportDto>, IEnumerable<FeesReportViewModel>>(feesReportDto);
                return Ok(feesReportVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
