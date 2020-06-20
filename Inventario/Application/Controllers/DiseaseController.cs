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
    [Route("api/diseases")]
    public class DiseaseController : Controller
    {
        private readonly IClientUseCases _diseaseUseCases;
        private readonly IMapper _mapper;

        public DiseaseController(IClientUseCases diseaseUseCases, IMapper mapper)
        {
            _diseaseUseCases = diseaseUseCases;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ClientDto> diseaseDto = await _diseaseUseCases.GetAll();
            IEnumerable<DiseaseViewModel> diseaseVM = _mapper.Map<IEnumerable<ClientDto>, IEnumerable<DiseaseViewModel>>(diseaseDto);
            return Ok(diseaseVM);
        }

        [HttpGet("{diseaseId}", Name = "GetDisease")]
        public async Task<IActionResult> GetOne(Guid diseaseId)
        {
            try
            {
                var diseaseDto = await _diseaseUseCases.GetOne(diseaseId);
                var diseaseVM = _mapper.Map<ClientDto, DiseaseViewModel>(diseaseDto);
                return Ok(diseaseVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(DiseaseForCreationViewModel diseaseForCreationVM)
        {
            var diseaseForCreationDto = _mapper.Map<DiseaseForCreationViewModel, ClientForCreationDto>(diseaseForCreationVM);
            var diseaseDto = await _diseaseUseCases.Create(diseaseForCreationDto);
            var diseaseVm = _mapper.Map<ClientDto, DiseaseViewModel>(diseaseDto);

            return CreatedAtRoute(
                "GetDisease",
                new { diseaseId = diseaseVm.Id },
                diseaseVm
            );
        }

        [HttpDelete("{diseaseId}")]
        public async Task<IActionResult> Delete(Guid diseaseId)
        {
            try
            {
                await _diseaseUseCases.Delete(diseaseId);
                return Ok("The disease was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{diseaseId}")]
        public async Task<IActionResult> Update(Guid diseaseId, [FromBody] DiseaseViewModel diseaseVM)
        {
            try
            {
                var diseaseDto = _mapper.Map<DiseaseViewModel, ClientDto>(diseaseVM);
                await _diseaseUseCases.Update(diseaseId, diseaseDto);
                return Ok("Disease successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
