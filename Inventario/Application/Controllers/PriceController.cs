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
    [Route("api/prices")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PriceController : Controller
    {
        private readonly IPriceUseCases _priceUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public PriceController(IPriceUseCases priceUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _priceUseCases = priceUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<PriceDto> priceDto = _priceUseCases.GetAll();
            IEnumerable<PriceViewModel> priceVM = _mapper.Map<IEnumerable<PriceDto>, IEnumerable<PriceViewModel>>(priceDto);
            return Ok(priceVM);
        }

        [HttpGet("{priceId}", Name = "GetPrice")]
        public async Task<IActionResult> GetOne(Guid priceId)
        {
            try
            {
                var priceDto = await _priceUseCases.GetOne(priceId);
                var priceVM = _mapper.Map<PriceDto, PriceViewModel>(priceDto);
                return Ok(priceVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(PriceForCreationViewModel priceForCreationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var priceForCreationDto = _mapper.Map<PriceForCreationViewModel, PriceForCreationDto>(priceForCreationVM);
            var priceDto = await _priceUseCases.Create(userId, priceForCreationDto);
            var priceVM = _mapper.Map<PriceDto, PriceViewModel>(priceDto);

            return CreatedAtRoute(
                "GetPrice",
                new { priceId = priceVM.Id },
                priceVM
            );
        }

        [HttpDelete("{priceId}")]
        public async Task<IActionResult> Delete(Guid priceId)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _priceUseCases.Delete(userId, priceId);
                return Ok("The animal-disease was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{priceId}")]
        public async Task<IActionResult> Update(Guid priceId, [FromBody] PriceViewModel priceVM)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var priceDto = _mapper.Map<PriceViewModel, PriceDto>(priceVM);
                priceDto.LastModificationBy = userId;
                await _priceUseCases.Update(priceId, priceDto);
                return Ok("Price successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
