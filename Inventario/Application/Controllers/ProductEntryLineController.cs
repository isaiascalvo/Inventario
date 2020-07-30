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
    [Route("api/product-entry-lines")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductEntryLineLineController: Controller
    {
        private readonly IProductEntryLineUseCases _productEntryLineUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProductEntryLineLineController(IProductEntryLineUseCases productEntryLineUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _productEntryLineUseCases = productEntryLineUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<ProductEntryLineDto> productEntryLinesDto = await _productEntryLineUseCases.GetAll();
                IEnumerable<ProductEntryLineViewModel> productEntryLinesVM = _mapper.Map<IEnumerable<ProductEntryLineDto>, IEnumerable<ProductEntryLineViewModel>>(productEntryLinesDto);
                return Ok(productEntryLinesVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{productEntryLineId}", Name = "GetProductEntryLine")]
        public async Task<IActionResult> GetOne(Guid productEntryLineId)
        {
            try
            {
                var productEntryLineDto = await _productEntryLineUseCases.GetOne(productEntryLineId);
                var productEntryLineVM = _mapper.Map<ProductEntryLineDto, ProductEntryLineViewModel>(productEntryLineDto);
                return Ok(productEntryLineVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductEntryLineForCreationViewModel productEntryLineForCerationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var productEntryLineForCerationDto = _mapper.Map<ProductEntryLineForCreationViewModel, ProductEntryLineForCreationDto>(productEntryLineForCerationVM);
            var productEntryLineDto = await _productEntryLineUseCases.Create(userId, productEntryLineForCerationDto);
            var productEntryLineVM = _mapper.Map<ProductEntryLineDto, ProductEntryLineViewModel>(productEntryLineDto);

            return CreatedAtRoute(
                "GetProductEntryLine",
                new { productEntryLineId = productEntryLineVM.Id },
                productEntryLineVM
            );
        }

        [HttpDelete("{productEntryLineId}")]
        public async Task<IActionResult> Delete(Guid productEntryLineId)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _productEntryLineUseCases.Delete(userId, productEntryLineId);
                return Ok("The Product Entry Line was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{productEntryLineId}")]
        public async Task<IActionResult> Update(Guid productEntryLineId, [FromBody] ProductEntryLineViewModel productEntryLineVM)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var productEntryLineDto = _mapper.Map<ProductEntryLineViewModel, ProductEntryLineDto>(productEntryLineVM);
                productEntryLineDto.LastModificationBy = userId;
                await _productEntryLineUseCases.Update(productEntryLineId, productEntryLineDto);
                return Ok("Product Entry Line successfully updated");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
