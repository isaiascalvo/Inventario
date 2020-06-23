using Application.ViewModels;
using AutoMapper;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/vendors")]
    public class VendorController : Controller
    {
        private readonly IVendorUseCases _vendorUseCases;
        private readonly IMapper _mapper;

        public VendorController(IVendorUseCases vendorUseCases, IMapper mapper)
        {
            _vendorUseCases = vendorUseCases;
            _mapper = mapper;
        }

        [AllowAnonymous]
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
            var vendorForCrationDto = _mapper.Map<VendorForCreationViewModel, VendorForCreationDto>(vendorForCrationVM);
            var vendorDto = await _vendorUseCases.Create(vendorForCrationDto);
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
                await _vendorUseCases.Delete(vendorId);
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
                var vendorDto = _mapper.Map<VendorViewModel, VendorDto>(vendorVM);
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
