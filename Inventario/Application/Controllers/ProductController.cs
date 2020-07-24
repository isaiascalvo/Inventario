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
    [Route("api/products")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController: Controller
    {
        private readonly IProductUseCases _productUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProductController(IProductUseCases productUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _productUseCases = productUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<ProductDto> productsDto = await _productUseCases.GetAll();
                IEnumerable<ProductViewModel> productsVM = _mapper.Map<IEnumerable<ProductDto>, IEnumerable<ProductViewModel>>(productsDto);
                return Ok(productsVM);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{categoryId}/ByCategory")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            IEnumerable<ProductDto> productsDto = await _productUseCases.GetByCategory(categoryId);
            IEnumerable<ProductViewModel> productsVM = _mapper.Map<IEnumerable<ProductDto>, IEnumerable<ProductViewModel>>(productsDto);
            return Ok(productsVM);
        }

        [HttpGet("{productId}", Name = "GetProduct")]
        public async Task<IActionResult> GetOne(Guid productId)
        {
            try
            {
                var productDto = await _productUseCases.GetOne(productId);
                var productVM = _mapper.Map<ProductDto, ProductViewModel>(productDto);
                return Ok(productVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductForCreationViewModel productForCreationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var productForCreationDto = _mapper.Map<ProductForCreationViewModel, ProductForCreationDto>(productForCreationVM);
            var product = await _productUseCases.Create(userId, productForCreationDto);
            var productVM = _mapper.Map<ProductDto, ProductViewModel>(product);

            return CreatedAtRoute(
                "GetProduct",
                new { productId = productVM.Id },
                productVM
            );
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(Guid productId)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _productUseCases.Delete(userId, productId);
                return Ok("The Product was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> Update(Guid productId, [FromBody] ProductViewModel productVM)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var productDto = _mapper.Map<ProductViewModel, ProductDto>(productVM);
                productDto.LastModificationBy = userId;
                await _productUseCases.Update(productId, productDto);
                return Ok("Product successfully updated");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
