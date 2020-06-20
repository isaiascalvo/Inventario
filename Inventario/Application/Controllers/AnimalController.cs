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

namespace Application
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/animals")]
    public class AnimalController: Controller
    {
        private readonly IProductUseCases _animalUseCases;
        private readonly IMapper _mapper;

        public AnimalController(IProductUseCases animalUseCases, IMapper mapper)
        {
            _animalUseCases = animalUseCases;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ProductDto> animalsDto = await _animalUseCases.GetAll();
            IEnumerable<AnimalViewModel> animalsVM = _mapper.Map<IEnumerable<ProductDto>, IEnumerable<AnimalViewModel>>(animalsDto);
            return Ok(animalsVM);
        }

        [HttpGet("{raceId}/ByRace")]
        public async Task<IActionResult> GetBySpecies(Guid raceId)
        {
            IEnumerable<ProductDto> animalsDto = await _animalUseCases.GetByRace(raceId);
            IEnumerable<AnimalViewModel> animalsVM = _mapper.Map<IEnumerable<ProductDto>, IEnumerable<AnimalViewModel>>(animalsDto);
            return Ok(animalsVM);
        }

        [HttpGet("{animalId}", Name = "GetAnimal")]
        public async Task<IActionResult> GetOne(Guid animalId)
        {
            try
            {
                var animalDto = await _animalUseCases.GetOne(animalId);
                var animalVM = _mapper.Map<ProductDto, AnimalViewModel>(animalDto);
                return Ok(animalVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AnimalForCreationVM animalForCreationVM)
        {
            var animalForCreationDto = _mapper.Map<AnimalForCreationVM, ProductForCreationDto>(animalForCreationVM);
            var animal = await _animalUseCases.Create(animalForCreationDto);
            var animalVm = _mapper.Map<ProductDto, AnimalViewModel>(animal);

            return CreatedAtRoute(
                "GetAnimal",
                new { animalId = animalVm.Id },
                animalVm
            );
        }

        [HttpDelete("{animalId}")]
        public async Task<IActionResult> Delete(Guid animalId)
        {
            try
            {
                await _animalUseCases.Delete(animalId);
                return Ok("The animal was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{animalId}")]
        public async Task<IActionResult> Update(Guid animalId, [FromBody] AnimalViewModel animalVM)
        {
            try
            {
                var animalDto = _mapper.Map<AnimalViewModel, ProductDto>(animalVM);
                await _animalUseCases.Update(animalId, animalDto);
                return Ok("Animal successfully updated");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
