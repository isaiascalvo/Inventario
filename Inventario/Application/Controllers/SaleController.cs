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
    [Route("api/sales")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SaleController : Controller
    {
        private readonly ISaleUseCases _saleUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public SaleController(ISaleUseCases saleUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _saleUseCases = saleUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<SaleDto> salesDto = await _saleUseCases.GetAll();
                IEnumerable<SaleViewModel> salesVM = _mapper.Map<IEnumerable<SaleDto>, IEnumerable<SaleViewModel>>(salesDto);
                //foreach (var s in salesVM)
                //{
                //    switch (s.PaymentType)
                //    {
                //        case Util.Enums.ePaymentTypes.Cash:
                //            s.Payment = _mapper.Map<CashDto, CashViewModel>((CashDto)salesDto.FirstOrDefault(x => x.Id == s.Id).Payment);
                //            break;
                //        case Util.Enums.ePaymentTypes.OwnFees:
                //            s.Payment = _mapper.Map<OwnFeesDto, OwnFeesViewModel>((OwnFeesDto)salesDto.FirstOrDefault(x => x.Id == s.Id).Payment);
                //            break;
                //        case Util.Enums.ePaymentTypes.CreditCard:
                //            s.Payment = _mapper.Map<CreditCardDto, CreditCardViewModel>((CreditCardDto)salesDto.FirstOrDefault(x => x.Id == s.Id).Payment);
                //            break;
                //        case Util.Enums.ePaymentTypes.DebitCard:
                //            s.Payment = _mapper.Map<DebitCardDto, DebitCardViewModel>((DebitCardDto)salesDto.FirstOrDefault(x => x.Id == s.Id).Payment);
                //            break;
                //        case Util.Enums.ePaymentTypes.Cheque:
                //            s.Payment = _mapper.Map<ChequeDto, ChequeViewModel>((ChequeDto)salesDto.FirstOrDefault(x => x.Id == s.Id).Payment);
                //            break;
                //        default:
                //            break;
                //    }
                //}
                return Ok(salesVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //[HttpGet("Filtered")]
        //public async Task<IActionResult> GetByFilters([FromQuery] SaleFiltersViewModel filtersVM)
        //{
        //    try
        //    {
        //        var filtersDto = _mapper.Map<SaleFiltersViewModel, SaleFiltersDto>(filtersVM);
        //        IEnumerable<SaleDto> salesDto = await _saleUseCases.GetByFilters(filtersDto);
        //        IEnumerable<SaleViewModel> salesVM = _mapper.Map<IEnumerable<SaleDto>, IEnumerable<SaleViewModel>>(salesDto);
        //        return Ok(salesVM);
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        [HttpGet("{saleId}", Name = "GetSale")]
        public async Task<IActionResult> GetOne(Guid saleId)
        {
            try
            {
                var saleDto = await _saleUseCases.GetOne(saleId);
                var saleVM = _mapper.Map<SaleDto, SaleViewModel>(saleDto);
                return Ok(saleVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(SaleForCreationViewModel saleForCreationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));
            var saleAndPaymentForCreationDto = _mapper.Map<SaleForCreationViewModel, SaleForCreationDto>(saleForCreationVM);
            var sale = await _saleUseCases.Create(userId, saleAndPaymentForCreationDto);
            var saleVM = _mapper.Map<SaleDto, SaleViewModel>(sale);

            return CreatedAtRoute(
                "GetSale",
                new { saleId = saleVM.Id },
                saleVM
            );
        }

        [HttpDelete("{saleId}")]
        public async Task<IActionResult> Delete(Guid saleId)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _saleUseCases.Delete(userId, saleId);
                return Ok("The Sale was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        //[HttpPut("{saleId}")]
        //public async Task<IActionResult> Update(Guid saleId, [FromBody] SaleViewModel saleVM)
        //{
        //    try
        //    {
        //        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

        //        var saleDto = _mapper.Map<SaleViewModel, SaleDto>(saleVM);
        //        saleDto.LastModificationBy = userId;
        //        await _saleUseCases.Update(saleId, saleDto);
        //        return Ok("Sale successfully updated");
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}
    }
}
