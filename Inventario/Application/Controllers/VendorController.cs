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
    [Route("api/vendors")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VendorController : Controller
    {
        private readonly IVendorUseCases _vendorUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public VendorController(IVendorUseCases vendorUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _vendorUseCases = vendorUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<VendorDto> vendorDto = await _vendorUseCases.GetAll();
            IEnumerable<VendorViewModel> vendorVM = _mapper.Map<IEnumerable<VendorDto>, IEnumerable<VendorViewModel>>(vendorDto);
            return Ok(vendorVM);
        }

        [HttpGet("{vendorId}", Name = "GetVendor")]
        public async Task<IActionResult> GetOne(Guid vendorId)
        {
            try
            {
                var vendorDto = await _vendorUseCases.GetOne(vendorId);
                var vendorVM = _mapper.Map<VendorDto, VendorViewModel>(vendorDto);
                return Ok(vendorVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("GetTotalQty")]
        public async Task<IActionResult> GetTotalQty()
        {
            try
            {
                int qty = await _vendorUseCases.GetTotalQty();
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
                IEnumerable<VendorDto> vendorsDto = await _vendorUseCases.GetByPageAndQty(skip, qty);
                IEnumerable<VendorViewModel> vendorsVM = _mapper.Map<IEnumerable<VendorDto>, IEnumerable<VendorViewModel>>(vendorsDto);
                return Ok(vendorsVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpGet("GetTotalQtyByFilters")]
        public async Task<IActionResult> GetTotalQtyByFilters([FromQuery] VendorFiltersViewModel filtersVM)
        {
            try
            {
                var filtersDto = _mapper.Map<VendorFiltersViewModel, VendorFiltersDto>(filtersVM);
                int qty = await _vendorUseCases.GetTotalQtyByFilters(filtersDto);
                return Ok(qty);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetByFiltersPageAndQty")]
        public async Task<IActionResult> GetByFiltersPageAndQty([FromQuery] VendorFiltersViewModel filtersVM, int skip, int qty)
        {
            try
            {
                var filtersDto = _mapper.Map<VendorFiltersViewModel,VendorFiltersDto>(filtersVM);
                IEnumerable<VendorDto> vendorsDto = await _vendorUseCases.GetFilteredByPageAndQty(filtersDto, skip, qty);
                IEnumerable<VendorViewModel> vendorsVM = _mapper.Map<IEnumerable<VendorDto>, IEnumerable<VendorViewModel>>(vendorsDto);
                return Ok(vendorsVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(VendorForCreationViewModel vendorForCrationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var vendorForCrationDto = _mapper.Map<VendorForCreationViewModel, VendorForCreationDto>(vendorForCrationVM);
            var vendorDto = await _vendorUseCases.Create(userId, vendorForCrationDto);
            var vendorVM = _mapper.Map<VendorDto, VendorViewModel>(vendorDto);

            return CreatedAtRoute(
                "GetVendor",
                new { vendorId = vendorVM.Id },
                vendorVM
            );
        }

        [HttpDelete("{vendorId}")]
        public async Task<IActionResult> Delete(Guid vendorId)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));
                await _vendorUseCases.Delete(userId, vendorId);
                return Ok("The Vendor was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{vendorId}")]
        public async Task<IActionResult> Update(Guid vendorId, [FromBody] VendorViewModel vendorVM)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var vendorDto = _mapper.Map<VendorViewModel, VendorDto>(vendorVM);
                vendorDto.LastModificationBy = userId;
                await _vendorUseCases.Update(vendorId, vendorDto);
                return Ok("Vendor successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
