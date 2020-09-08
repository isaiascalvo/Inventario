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
    [Route("api/miscellaneous-expenses")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MiscellaneousExpensesController : Controller
    {
        private readonly IMiscellaneousExpensesUseCases _miscellaneousExpensesUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public MiscellaneousExpensesController(IMiscellaneousExpensesUseCases miscellaneousExpensesUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _miscellaneousExpensesUseCases = miscellaneousExpensesUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<MiscellaneousExpensesDto> miscellaneousExpensesDto = await _miscellaneousExpensesUseCases.GetAll();
            IEnumerable<MiscellaneousExpensesViewModel> categories = _mapper.Map<IEnumerable<MiscellaneousExpensesDto>, IEnumerable<MiscellaneousExpensesViewModel>>(miscellaneousExpensesDto);
            return Ok(categories);
        }

        [HttpGet("GetTotalQty")]
        public async Task<IActionResult> GetTotalQty()
        {
            try
            {
                int qty = await _miscellaneousExpensesUseCases.GetTotalQty();
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
                IEnumerable<MiscellaneousExpensesDto> miscellaneousExpensesDto = await _miscellaneousExpensesUseCases.GetByPageAndQty(skip, qty);
                IEnumerable<MiscellaneousExpensesViewModel> miscellaneousExpensesVM = _mapper.Map<IEnumerable<MiscellaneousExpensesDto>, IEnumerable<MiscellaneousExpensesViewModel>>(miscellaneousExpensesDto);
                return Ok(miscellaneousExpensesVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpGet("GetTotalQtyByFilters")]
        public async Task<IActionResult> GetTotalQtyByFilters([FromQuery] MiscellaneousExpensesFiltersViewModel filtersVM)
        {
            try
            {
                var filtersDto = _mapper.Map<MiscellaneousExpensesFiltersViewModel, MiscellaneousExpensesFiltersDto>(filtersVM);
                int qty = await _miscellaneousExpensesUseCases.GetTotalQtyByFilters(filtersDto);
                return Ok(qty);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetByFiltersPageAndQty")]
        public async Task<IActionResult> GetByFiltersPageAndQty([FromQuery] MiscellaneousExpensesFiltersViewModel filtersVM, int skip, int qty)
        {
            try
            {
                var filtersDto = _mapper.Map<MiscellaneousExpensesFiltersViewModel, MiscellaneousExpensesFiltersDto>(filtersVM);
                IEnumerable<MiscellaneousExpensesDto> miscellaneousExpensesDto = await _miscellaneousExpensesUseCases.GetFilteredByPageAndQty(filtersDto, skip, qty);
                IEnumerable<MiscellaneousExpensesViewModel> miscellaneousExpensesVM = _mapper.Map<IEnumerable<MiscellaneousExpensesDto>, IEnumerable<MiscellaneousExpensesViewModel>>(miscellaneousExpensesDto);
                return Ok(miscellaneousExpensesVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{miscellaneousExpensesId}", Name = "GetMiscellaneousExpenses")]
        public async Task<IActionResult> GetOne(Guid miscellaneousExpensesId)
        {
            try
            {
                var miscellaneousExpensesDto = await _miscellaneousExpensesUseCases.GetOne(miscellaneousExpensesId);
                var miscellaneousExpensesVM = _mapper.Map<MiscellaneousExpensesDto, MiscellaneousExpensesViewModel>(miscellaneousExpensesDto);
                return Ok(miscellaneousExpensesVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(MiscellaneousExpensesForCreationDto miscellaneousExpensesForCreationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var miscellaneousExpensesForCreationDto = _mapper.Map<MiscellaneousExpensesForCreationDto, MiscellaneousExpensesForCreationDto>(miscellaneousExpensesForCreationVM);
            var miscellaneousExpensesDto = await _miscellaneousExpensesUseCases.Create(userId, miscellaneousExpensesForCreationDto);
            var miscellaneousExpensesVM = _mapper.Map<MiscellaneousExpensesDto, MiscellaneousExpensesViewModel>(miscellaneousExpensesDto);

            return CreatedAtRoute(
                "GetMiscellaneousExpenses",
                new { miscellaneousExpensesId = miscellaneousExpensesVM.Id },
                miscellaneousExpensesVM
            );
        }

        [HttpDelete("{miscellaneousExpensesId}")]
        public async Task<IActionResult> Delete(Guid miscellaneousExpensesId)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _miscellaneousExpensesUseCases.Delete(userId, miscellaneousExpensesId);
                return Ok("The miscellaneous expense was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{miscellaneousExpensesId}")]
        public async Task<IActionResult> Update(Guid miscellaneousExpensesId, [FromBody] MiscellaneousExpensesViewModel miscellaneousExpenses)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var miscellaneousExpensesDto = _mapper.Map<MiscellaneousExpensesViewModel, MiscellaneousExpensesDto>(miscellaneousExpenses);
                miscellaneousExpensesDto.LastModificationBy = userId;
                await _miscellaneousExpensesUseCases.Update(miscellaneousExpensesId, miscellaneousExpensesDto);
                return Ok("Miscellaneous Expense successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
