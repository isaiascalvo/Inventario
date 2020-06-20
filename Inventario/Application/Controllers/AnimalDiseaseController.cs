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
    [Route("api/animals-diseases")]
    public class AnimalDiseaseController : Controller
    {
        private readonly IAnimalDiseaseUseCases _animalDiseaseUseCases;
        private readonly IMapper _mapper;

        public AnimalDiseaseController(IAnimalDiseaseUseCases animalDiseaseUseCases, IMapper mapper)
        {
            _animalDiseaseUseCases = animalDiseaseUseCases;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<AnimalDiseaseDto> animalDiseaseDto = await _animalDiseaseUseCases.GetAll();
            IEnumerable<AnimalDiseaseViewModel> animalDiseaseVM = _mapper.Map<IEnumerable<AnimalDiseaseDto>, IEnumerable<AnimalDiseaseViewModel>>(animalDiseaseDto);
            return Ok(animalDiseaseVM);
        }

        [HttpGet("{animalDiseaseId}", Name = "GetAnimalDisease")]
        public async Task<IActionResult> GetOne(Guid animalDiseaseId)
        {
            try
            {
                var animalDiseaseDto = await _animalDiseaseUseCases.GetOne(animalDiseaseId);
                var animalDiseaseVM = _mapper.Map<AnimalDiseaseDto, AnimalDiseaseViewModel>(animalDiseaseDto);
                return Ok(animalDiseaseVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AnimalDiseaseForCreationViewModel animalDiseaseForCreationVM)
        {
            var animalDiseaseForCreationDto = _mapper.Map<AnimalDiseaseForCreationViewModel, AnimalDiseaseForCreationDto>(animalDiseaseForCreationVM);
            var animalDiseaseDto = await _animalDiseaseUseCases.Create(animalDiseaseForCreationDto);
            var animalDiseaseVM = _mapper.Map<AnimalDiseaseDto, AnimalDiseaseViewModel>(animalDiseaseDto);

            return CreatedAtRoute(
                "GetAnimalDisease",
                new { animalDiseaseId = animalDiseaseVM.Id },
                animalDiseaseVM
            );
        }

        [HttpDelete("{animalDiseaseId}")]
        public async Task<IActionResult> Delete(Guid animalDiseaseId)
        {
            try
            {
                await _animalDiseaseUseCases.Delete(animalDiseaseId);
                return Ok("The animal-disease was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{animalDiseaseId}")]
        public async Task<IActionResult> Update(Guid animalDiseaseId, [FromBody] AnimalDiseaseViewModel animalDiseaseVM)
        {
            try
            {
                var animalDiseaseDto = _mapper.Map<AnimalDiseaseViewModel, AnimalDiseaseDto>(animalDiseaseVM);
                await _animalDiseaseUseCases.Update(animalDiseaseId, animalDiseaseDto);
                return Ok("AnimalDisease successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
