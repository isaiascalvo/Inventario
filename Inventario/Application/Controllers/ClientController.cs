﻿using Application.ViewModels;
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
    [Route("api/clients")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientController : Controller
    {
        private readonly IClientUseCases _clientUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ClientController(IClientUseCases clientUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _clientUseCases = clientUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ClientDto> clientsDto = await _clientUseCases.GetAll();
            IEnumerable<ClientViewModel> clientsVM = _mapper.Map<IEnumerable<ClientDto>, IEnumerable<ClientViewModel>>(clientsDto);
            return Ok(clientsVM);
        }

        [HttpGet("Filtered")]
        public async Task<IActionResult> GetByFilters([FromQuery] ClientFiltersViewModel filtersVM)
        {
            var filtersDto = _mapper.Map<ClientFiltersViewModel, ClientFiltersDto>(filtersVM);
            IEnumerable<ClientDto> clientsDto = await _clientUseCases.GetByFilters(filtersDto);
            IEnumerable<ClientViewModel> clientsVM = _mapper.Map<IEnumerable<ClientDto>, IEnumerable<ClientViewModel>>(clientsDto);
            return Ok(clientsVM);
        }

        [HttpGet("GetTotalQty")]
        public async Task<IActionResult> GetTotalQty()
        {
            try
            {
                int qty = await _clientUseCases.GetTotalQty();
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
                IEnumerable<ClientDto> clientsDto = await _clientUseCases.GetByPageAndQty(skip, qty);
                IEnumerable<ClientViewModel> clientsVM = _mapper.Map<IEnumerable<ClientDto>, IEnumerable<ClientViewModel>>(clientsDto);
                return Ok(clientsVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpGet("GetTotalQtyByFilters")]
        public async Task<IActionResult> GetTotalQtyByFilters([FromQuery] ClientFiltersViewModel filtersVM)
        {
            try
            {
                var filtersDto = _mapper.Map<ClientFiltersViewModel, ClientFiltersDto>(filtersVM);
                int qty = await _clientUseCases.GetTotalQtyByFilters(filtersDto);
                return Ok(qty);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetByFiltersPageAndQty")]
        public async Task<IActionResult> GetByFiltersPageAndQty([FromQuery] ClientFiltersViewModel filtersVM, int skip, int qty)
        {
            try
            {
                var filtersDto = _mapper.Map<ClientFiltersViewModel, ClientFiltersDto>(filtersVM);
                IEnumerable<ClientDto> clientsDto = await _clientUseCases.GetFilteredByPageAndQty(filtersDto, skip, qty);
                IEnumerable<ClientViewModel> clientsVM = _mapper.Map<IEnumerable<ClientDto>, IEnumerable<ClientViewModel>>(clientsDto);
                return Ok(clientsVM);
            }
            catch (Exception e)
            {
                throw e;
            }
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
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var clientForCreationDto = _mapper.Map<ClientForCreationViewModel, ClientForCreationDto>(clientForCreationVM);
            var clientDto = await _clientUseCases.Create(userId, clientForCreationDto);
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
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _clientUseCases.Delete(userId, clientId);
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
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var clientDto = _mapper.Map<ClientViewModel, ClientDto>(clientVM);
                clientDto.LastModificationBy = userId;
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
