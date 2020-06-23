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
    [Route("api/clients")]
    public class ClientController : Controller
    {
        private readonly IClientUseCases _clientUseCases;
        private readonly IMapper _mapper;

        public ClientController(IClientUseCases clientUseCases, IMapper mapper)
        {
            _clientUseCases = clientUseCases;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ClientDto> clientsDto = await _clientUseCases.GetAll();
            IEnumerable<ClientViewModel> clientsVM = _mapper.Map<IEnumerable<ClientDto>, IEnumerable<ClientViewModel>>(clientsDto);
            return Ok(clientsVM);
        }

        [HttpGet("{clientId}", Name = "GetClient")]
        public async Task<IActionResult> GetOne(Guid clientId)
        {
            try
            {
                var clientDto = await _clientUseCases.GetOne(clientId);
                var clientVM = _mapper.Map<ClientDto, ClientViewModel>(clientDto);
                return Ok(clientVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ClientForCreationViewModel clientForCreationVM)
        {
            var clientForCreationDto = _mapper.Map<ClientForCreationViewModel, ClientForCreationDto>(clientForCreationVM);
            var clientDto = await _clientUseCases.Create(clientForCreationDto);
            var clientVM = _mapper.Map<ClientDto, ClientViewModel>(clientDto);

            return CreatedAtRoute(
                "GetClient",
                new { clientId = clientVM.Id },
                clientVM
            );
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> Delete(Guid clientId)
        {
            try
            {
                await _clientUseCases.Delete(clientId);
                return Ok("The client was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{clientId}")]
        public async Task<IActionResult> Update(Guid clientId, [FromBody] ClientViewModel clientVM)
        {
            try
            {
                var clientDto = _mapper.Map<ClientViewModel, ClientDto>(clientVM);
                await _clientUseCases.Update(clientId, clientDto);
                return Ok("Client successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
