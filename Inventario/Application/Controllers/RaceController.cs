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
    [Route("api/races")]
    public class RaceController : Controller
    {
        private readonly ICategoryUseCases _raceUseCases;
        private readonly IMapper _mapper;

        public RaceController(ICategoryUseCases raceUseCases, IMapper mapper)
        {
            _raceUseCases = raceUseCases;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CategoryDto> racesDto = await _raceUseCases.GetAll();
            IEnumerable<RaceViewModel> racesVM = _mapper.Map<IEnumerable<CategoryDto>, IEnumerable<RaceViewModel>>(racesDto);
            return Ok(racesVM);
        }

        [HttpGet("{raceId}", Name = "GetRace")]
        public async Task<IActionResult> GetOne(Guid raceId)
        {
            try
            {
                var raceDto = await _raceUseCases.GetOne(raceId);
                var raceVM = _mapper.Map<CategoryDto, RaceViewModel>(raceDto);
                return Ok(raceVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(RaceForCreationViewModel raceForCreationVM)
        {
            var raceForCreationDto = _mapper.Map<RaceForCreationViewModel, CategoryForCreationDto>(raceForCreationVM);
            var raceDto = await _raceUseCases.Create(raceForCreationDto);
            var raceVM = _mapper.Map<CategoryDto, RaceViewModel>(raceDto);

            return CreatedAtRoute(
                "GetRace",
                new { raceId = raceVM.Id },
                raceVM
            );
        }

        [HttpDelete("{raceId}")]
        public async Task<IActionResult> Delete(Guid raceId)
        {
            try
            {
                await _raceUseCases.Delete(raceId);
                return Ok("The race was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{raceId}")]
        public async Task<IActionResult> Update(Guid raceId, [FromBody] RaceViewModel raceVM)
        {
            try
            {
                var raceDto = _mapper.Map<RaceViewModel, CategoryDto>(raceVM);
                await _raceUseCases.Update(raceId, raceDto);
                return Ok("Race successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
