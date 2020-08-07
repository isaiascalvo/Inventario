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

namespace Application
{
    [ApiController]
    [Route("api/purchases")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PurchaseController: Controller
    {
        private readonly IPurchaseUseCases _purchaseUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public PurchaseController(IPurchaseUseCases purchaseUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _purchaseUseCases = purchaseUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<PurchaseDto> purchasesDto = await _purchaseUseCases.GetAll();
                IEnumerable<PurchaseViewModel> purchasesVM = _mapper.Map<IEnumerable<PurchaseDto>, IEnumerable<PurchaseViewModel>>(purchasesDto);
                return Ok(purchasesVM);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        //[HttpGet("Filtered")]
        //public async Task<IActionResult> GetByFilters([FromQuery] PurchaseFiltersViewModel filtersVM)
        //{
        //    try
        //    {
        //        var filtersDto = _mapper.Map<PurchaseFiltersViewModel, PurchaseFiltersDto>(filtersVM);
        //        IEnumerable<PurchaseDto> purchasesDto = await _purchaseUseCases.GetByFilters(filtersDto);
        //        IEnumerable<PurchaseViewModel> purchasesVM = _mapper.Map<IEnumerable<PurchaseDto>, IEnumerable<PurchaseViewModel>>(purchasesDto);
        //        return Ok(purchasesVM);
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        [HttpGet("{purchaseId}", Name = "GetPurchase")]
        public async Task<IActionResult> GetOne(Guid purchaseId)
        {
            try
            {
                var purchaseDto = await _purchaseUseCases.GetOne(purchaseId);
                var purchaseVM = _mapper.Map<PurchaseDto, PurchaseViewModel>(purchaseDto);
                return Ok(purchaseVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(PurchaseForCreationViewModel purchaseForCreationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var purchaseForCreationDto = _mapper.Map<PurchaseForCreationViewModel, PurchaseForCreationDto>(purchaseForCreationVM);
            var purchase = await _purchaseUseCases.Create(userId, purchaseForCreationDto);
            var purchaseVM = _mapper.Map<PurchaseDto, PurchaseViewModel>(purchase);

            return CreatedAtRoute(
                "GetPurchase",
                new { purchaseId = purchaseVM.Id },
                purchaseVM
            );
        }

        [HttpDelete("{purchaseId}")]
        public async Task<IActionResult> Delete(Guid purchaseId)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _purchaseUseCases.Delete(userId, purchaseId);
                return Ok("The Purchase was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{purchaseId}")]
        public async Task<IActionResult> Update(Guid purchaseId, [FromBody] PurchaseViewModel purchaseVM)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var purchaseDto = _mapper.Map<PurchaseViewModel, PurchaseDto>(purchaseVM);
                purchaseDto.LastModificationBy = userId;
                await _purchaseUseCases.Update(purchaseId, purchaseDto);
                return Ok("Purchase successfully updated");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
