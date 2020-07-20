﻿using Application.ViewModels;
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
    [Route("api/prices")]
    public class PriceController : Controller
    {
        private readonly IPriceUseCases _animalDiseaseUseCases;
        private readonly IMapper _mapper;

        public PriceController(IPriceUseCases animalDiseaseUseCases, IMapper mapper)
        {
            _animalDiseaseUseCases = animalDiseaseUseCases;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<PriceDto> animalDiseaseDto = _animalDiseaseUseCases.GetAll();
            IEnumerable<PriceViewModel> animalDiseaseVM = _mapper.Map<IEnumerable<PriceDto>, IEnumerable<PriceViewModel>>(animalDiseaseDto);
            return Ok(animalDiseaseVM);
        }

        [HttpGet("{animalDiseaseId}", Name = "GetAnimalDisease")]
        public async Task<IActionResult> GetOne(Guid animalDiseaseId)
        {
            try
            {
                var animalDiseaseDto = await _animalDiseaseUseCases.GetOne(animalDiseaseId);
                var animalDiseaseVM = _mapper.Map<PriceDto, PriceViewModel>(animalDiseaseDto);
                return Ok(animalDiseaseVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(PriceForCreationViewModel animalDiseaseForCreationVM)
        {
            var animalDiseaseForCreationDto = _mapper.Map<PriceForCreationViewModel, PriceForCreationDto>(animalDiseaseForCreationVM);
            var animalDiseaseDto = await _animalDiseaseUseCases.Create(animalDiseaseForCreationDto);
            var animalDiseaseVM = _mapper.Map<PriceDto, PriceViewModel>(animalDiseaseDto);

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
        public async Task<IActionResult> Update(Guid animalDiseaseId, [FromBody] PriceViewModel animalDiseaseVM)
        {
            try
            {
                var animalDiseaseDto = _mapper.Map<PriceViewModel, PriceDto>(animalDiseaseVM);
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
