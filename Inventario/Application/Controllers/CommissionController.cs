using Application.ViewModels;
using AutoMapper;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/commissions")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommissionController : Controller
    {
        private readonly ICommissionUseCases _commissionUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CommissionController(ICommissionUseCases commissionUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _commissionUseCases = commissionUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CommissionDto> commissionsDto = await _commissionUseCases.GetAll();
            IEnumerable<CommissionViewModel> commissionsVM = _mapper.Map<IEnumerable<CommissionDto>, IEnumerable<CommissionViewModel>>(commissionsDto);
            return Ok(commissionsVM);
        }

        [HttpGet("GetTotalQty")]
        public async Task<IActionResult> GetTotalQty()
        {
            try
            {
                int qty = await _commissionUseCases.GetTotalQty();
                return Ok(qty);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetByPageAndQty")]
        public async Task<IActionResult> GetByPageAndQty(int skip, int qty)
        {
            try
            {
                IEnumerable<CommissionDto> commissionsDto = await _commissionUseCases.GetByPageAndQty(skip, qty);
                IEnumerable<CommissionViewModel> commissionsVM = _mapper.Map<IEnumerable<CommissionDto>, IEnumerable<CommissionViewModel>>(commissionsDto);
                return Ok(commissionsVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpGet("GetTotalQtyByFilters")]
        public async Task<IActionResult> GetTotalQtyByFilters([FromQuery] CommissionFiltersViewModel filtersVM)
        {
            try
            {
                var filtersDto = _mapper.Map<CommissionFiltersViewModel, CommissionFiltersDto>(filtersVM);
                int qty = await _commissionUseCases.GetTotalQtyByFilters(filtersDto);
                return Ok(qty);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetByFiltersPageAndQty")]
        public async Task<IActionResult> GetByFiltersPageAndQty([FromQuery] CommissionFiltersViewModel filtersVM, int skip, int qty)
        {
            try
            {
                var filtersDto = _mapper.Map<CommissionFiltersViewModel, CommissionFiltersDto>(filtersVM);
                IEnumerable<CommissionDto> commissionsDto = await _commissionUseCases.GetFilteredByPageAndQty(filtersDto, skip, qty);
                IEnumerable<CommissionViewModel> commissionsVM = _mapper.Map<IEnumerable<CommissionDto>, IEnumerable<CommissionViewModel>>(commissionsDto);
                return Ok(commissionsVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{commissionId}", Name = "GetCommission")]
        public async Task<IActionResult> GetOne(Guid commissionId)
        {
            try
            {
                var commissionDto = await _commissionUseCases.GetOne(commissionId);
                var commissionVM = _mapper.Map<CommissionDto, CommissionViewModel>(commissionDto);
                return Ok(commissionVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(CommissionForCreationViewModel commissionForCreationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var commissionForCreationDto = _mapper.Map<CommissionForCreationViewModel, CommissionForCreationDto>(commissionForCreationVM);
            var commissionDto = await _commissionUseCases.Create(userId, commissionForCreationDto);
            var commissionVM = _mapper.Map<CommissionDto, CommissionViewModel>(commissionDto);

            return CreatedAtRoute(
                "GetCommission",
                new { commissionId = commissionVM.Id },
                commissionVM
            );
        }

        [HttpDelete("{commissionId}")]
        public async Task<IActionResult> Delete(Guid commissionId)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _commissionUseCases.Delete(userId, commissionId);
                return Ok("The commission was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{commissionId}")]
        public async Task<IActionResult> Update(Guid commissionId, [FromBody] CommissionViewModel commissionVM)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var commissionDto = _mapper.Map<CommissionViewModel, CommissionDto>(commissionVM);
                commissionDto.LastModificationBy = userId;
                await _commissionUseCases.Update(commissionId, commissionDto);
                return Ok("Commission successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
