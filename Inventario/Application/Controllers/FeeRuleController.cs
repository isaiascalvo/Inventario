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
    [Route("api/fee-rules")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeeRuleController : Controller
    {
        private readonly IFeeRuleUseCases _feeRuleUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public FeeRuleController(IFeeRuleUseCases feeRuleUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _feeRuleUseCases = feeRuleUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<FeeRuleDto> feeRuleDto = await _feeRuleUseCases.GetAll();
                IEnumerable<FeeRuleViewModel> feeRuleVM = _mapper.Map<IEnumerable<FeeRuleDto>, IEnumerable<FeeRuleViewModel>>(feeRuleDto);
                return Ok(feeRuleVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetTotalQty")]
        public async Task<IActionResult> GetTotalQty()
        {
            try
            {
                int qty = await _feeRuleUseCases.GetTotalQty();
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
                IEnumerable<FeeRuleDto> feeRulesDto = await _feeRuleUseCases.GetByPageAndQty(skip, qty);
                IEnumerable<FeeRuleViewModel> feeRulesVM = _mapper.Map<IEnumerable<FeeRuleDto>, IEnumerable<FeeRuleViewModel>>(feeRulesDto);
                return Ok(feeRulesVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpGet("GetTotalQtyByFilters")]
        public async Task<IActionResult> GetTotalQtyByFilters([FromQuery] FeeRuleFiltersViewModel filtersVM)
        {
            try
            {
                var filtersDto = _mapper.Map<FeeRuleFiltersViewModel, FeeRuleFiltersDto>(filtersVM);
                int qty = await _feeRuleUseCases.GetTotalQtyByFilters(filtersDto);
                return Ok(qty);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetByFiltersPageAndQty")]
        public async Task<IActionResult> GetByFiltersPageAndQty([FromQuery] FeeRuleFiltersViewModel filtersVM, int skip, int qty)
        {
            try
            {
                var filtersDto = _mapper.Map<FeeRuleFiltersViewModel, FeeRuleFiltersDto>(filtersVM);
                IEnumerable<FeeRuleDto> feeRulesDto = await _feeRuleUseCases.GetFilteredByPageAndQty(filtersDto, skip, qty);
                IEnumerable<FeeRuleViewModel> feeRulesVM = _mapper.Map<IEnumerable<FeeRuleDto>, IEnumerable<FeeRuleViewModel>>(feeRulesDto);
                return Ok(feeRulesVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost("ByProducts")]
        public async Task<IActionResult> GetByProducts(List<Guid> productsIds)
        {
            try
            {
                IEnumerable<FeeRuleDto> feeRuleDto = await _feeRuleUseCases.GetByProducts(productsIds);
                IEnumerable<FeeRuleViewModel> feeRuleVM = _mapper.Map<IEnumerable<FeeRuleDto>, IEnumerable<FeeRuleViewModel>>(feeRuleDto);
                return Ok(feeRuleVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{feeRuleId}", Name = "GetFeeRule")]
        public async Task<IActionResult> GetOne(Guid feeRuleId)
        {
            try
            {
                var feeRuleDto = await _feeRuleUseCases.GetOne(feeRuleId);
                var feeRuleVM = _mapper.Map<FeeRuleDto, FeeRuleViewModel>(feeRuleDto);
                return Ok(feeRuleVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(FeeRuleForCreationViewModel feeRuleForCreationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var feeRuleForCreationDto = _mapper.Map<FeeRuleForCreationViewModel, FeeRuleForCreationDto>(feeRuleForCreationVM);
            var feeRule = await _feeRuleUseCases.Create(userId, feeRuleForCreationDto);
            var feeRuleVM = _mapper.Map<FeeRuleDto, FeeRuleViewModel>(feeRule);

            return CreatedAtRoute(
                "GetFeeRule",
                new { feeRuleId = feeRuleVM.Id },
                feeRuleVM
            );
        }

        [HttpDelete("{feeRuleId}")]
        public async Task<IActionResult> Delete(Guid feeRuleId)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _feeRuleUseCases.Delete(userId, feeRuleId);
                return Ok("The Fee Rule was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{feeRuleId}")]
        public async Task<IActionResult> Update(Guid feeRuleId, [FromBody] FeeRuleViewModel feeRuleVM)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var feeRuleDto = _mapper.Map<FeeRuleViewModel, FeeRuleDto>(feeRuleVM);
                feeRuleDto.LastModificationBy = userId;
                await _feeRuleUseCases.Update(feeRuleId, feeRuleDto);
                return Ok("Fee Rule successfully updated");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        //[AllowAnonymous]
        //[HttpGet("Pdf")]
        //public async Task<IActionResult> CreatePdf()
        //{
        //    try
        //    {
        //        MemoryStream stream = await _feeRuleUseCases.CreatePdf();
        //        stream.Position = 0;
        //        return File(stream, "application/pdf");
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        [HttpPost("ByCategory")]
        public async Task<IActionResult> AddByCategory(FeeRuleByCategoryViewModel feeRuleByCategoryVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var feeRuleByCategoryDto = _mapper.Map<FeeRuleByCategoryViewModel, FeeRuleByCategoryDto>(feeRuleByCategoryVM);
            await _feeRuleUseCases.CreateByCategory(userId, feeRuleByCategoryDto);
            return Ok("Fee Rules successfully created.");
        }
    }
}
