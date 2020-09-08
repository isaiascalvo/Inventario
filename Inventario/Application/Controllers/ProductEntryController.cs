using Application.ViewModels;
using AutoMapper;
using Logic;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/product-entries")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductEntryController: Controller
    {
        private readonly IProductEntryUseCases _productEntryUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProductEntryController(IProductEntryUseCases productEntryUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _productEntryUseCases = productEntryUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<ProductEntryDto> productEntriesDto = await _productEntryUseCases.GetAll();
                IEnumerable<ProductEntryViewModel> productEntriesVM = _mapper.Map<IEnumerable<ProductEntryDto>, IEnumerable<ProductEntryViewModel>>(productEntriesDto);
                return Ok(productEntriesVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetTotalQty")]
        public async Task<IActionResult> GetTotalQty()
        {
            try
            {
                int qty = await _productEntryUseCases.GetTotalQty();
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
                IEnumerable<ProductEntryDto> productEntriesDto = await _productEntryUseCases.GetByPageAndQty(skip, qty);
                IEnumerable<ProductEntryViewModel> productEntriesVM = _mapper.Map<IEnumerable<ProductEntryDto>, IEnumerable<ProductEntryViewModel>>(productEntriesDto);
                return Ok(productEntriesVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpGet("GetTotalQtyByFilters")]
        public async Task<IActionResult> GetTotalQtyByFilters([FromQuery] ProductEntryFiltersViewModel filtersVM)
        {
            try
            {
                var filtersDto = _mapper.Map<ProductEntryFiltersViewModel, ProductEntryFiltersDto>(filtersVM);
                int qty = await _productEntryUseCases.GetTotalQtyByFilters(filtersDto);
                return Ok(qty);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetByFiltersPageAndQty")]
        public async Task<IActionResult> GetByFiltersPageAndQty([FromQuery] ProductEntryFiltersViewModel filtersVM, int skip, int qty)
        {
            try
            {
                var filtersDto = _mapper.Map<ProductEntryFiltersViewModel, ProductEntryFiltersDto>(filtersVM);
                IEnumerable<ProductEntryDto> productEntriesDto = await _productEntryUseCases.GetFilteredByPageAndQty(filtersDto, skip, qty);
                IEnumerable<ProductEntryViewModel> productEntriesVM = _mapper.Map<IEnumerable<ProductEntryDto>, IEnumerable<ProductEntryViewModel>>(productEntriesDto);
                return Ok(productEntriesVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{productEntryId}", Name = "GetProductEntry")]
        public async Task<IActionResult> GetOne(Guid productEntryId)
        {
            try
            {
                var productEntryDto = await _productEntryUseCases.GetOne(productEntryId);
                var productEntryVM = _mapper.Map<ProductEntryDto, ProductEntryViewModel>(productEntryDto);
                return Ok(productEntryVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductEntryForCreationViewModel productForCreationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var productEntryForCreationDto = _mapper.Map<ProductEntryForCreationViewModel, ProductEntryForCreationDto>(productForCreationVM);
            var productEntry = await _productEntryUseCases.Create(userId, productEntryForCreationDto);
            var productEntryVM = _mapper.Map<ProductEntryDto, ProductEntryViewModel>(productEntry);

            return CreatedAtRoute(
                "GetProductEntry",
                new { productEntryId = productEntryVM.Id },
                productEntryVM
            );
        }

        [HttpDelete("{productEntryId}")]
        public async Task<IActionResult> Delete(Guid productEntryId)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _productEntryUseCases.Delete(userId, productEntryId);
                return Ok("The Product Entry was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{productEntryId}")]
        public async Task<IActionResult> Update(Guid productEntryId, [FromBody] ProductEntryViewModel productEntryVM)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var productEntryDto = _mapper.Map<ProductEntryViewModel, ProductEntryDto>(productEntryVM);
                productEntryDto.LastModificationBy = userId;
                await _productEntryUseCases.Update(productEntryId, productEntryDto);
                return Ok("Product Entry successfully updated");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
