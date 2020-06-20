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
    [Route("api/pregnancies")]
    public class PregnancyController : Controller
    {
        private readonly IVendorUseCases _pregnancyUseCases;
        private readonly IMapper _mapper;

        public PregnancyController(IVendorUseCases pregnancyUseCases, IMapper mapper)
        {
            _pregnancyUseCases = pregnancyUseCases;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<VendorDto> pregncnayDto = await _pregnancyUseCases.GetAll();
            IEnumerable<PregnancyViewModel> pregnancyVM = _mapper.Map<IEnumerable<VendorDto>, IEnumerable<PregnancyViewModel>>(pregncnayDto);
            return Ok(pregnancyVM);
        }

        [HttpGet("{pregnancyId}", Name = "GetPregnancy")]
        public async Task<IActionResult> GetOne(Guid pregnancyId)
        {
            try
            {
                var pregnancyDto = await _pregnancyUseCases.GetOne(pregnancyId);
                var pregnancyVM = _mapper.Map<VendorDto, PregnancyViewModel>(pregnancyDto);
                return Ok(pregnancyVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(PregnancyForCreationViewModel pregnancyForCrationVM)
        {
            var pregnancyForCrationDto = _mapper.Map<PregnancyForCreationViewModel, VendorForCreationDto>(pregnancyForCrationVM);
            var pregnancyDto = await _pregnancyUseCases.Create(pregnancyForCrationDto);
            var pregnancyVM = _mapper.Map<VendorDto, PregnancyViewModel>(pregnancyDto);

            return CreatedAtRoute(
                "GetPregnancy",
                new { pregnancyId = pregnancyVM.Id },
                pregnancyVM
            );
        }

        [HttpDelete("{pregnancyId}")]
        public async Task<IActionResult> Delete(Guid pregnancyId)
        {
            try
            {
                await _pregnancyUseCases.Delete(pregnancyId);
                return Ok("The pregnancy was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{pregnancyId}")]
        public async Task<IActionResult> Update(Guid pregnancyId, [FromBody] PregnancyViewModel pregnancyVM)
        {
            try
            {
                var pregnancyDto = _mapper.Map<PregnancyViewModel, VendorDto>(pregnancyVM);
                await _pregnancyUseCases.Update(pregnancyId, pregnancyDto);
                return Ok("Pregnancy successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
