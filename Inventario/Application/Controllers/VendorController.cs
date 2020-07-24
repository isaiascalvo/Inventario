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
