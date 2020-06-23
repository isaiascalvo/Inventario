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

namespace Application
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/products")]
    public class ProductController: Controller
    {
        private readonly IProductUseCases _productUseCases;
        private readonly IMapper _mapper;

        public ProductController(IProductUseCases productUseCases, IMapper mapper)
        {
            _productUseCases = productUseCases;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ProductDto> productsDto = await _productUseCases.GetAll();
            IEnumerable<ProductViewModel> productsVM = _mapper.Map<IEnumerable<ProductDto>, IEnumerable<ProductViewModel>>(productsDto);
            return Ok(productsVM);
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
            var productForCreationDto = _mapper.Map<ProductForCreationViewModel, ProductForCreationDto>(productForCreationVM);
            var product = await _productUseCases.Create(productForCreationDto);
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
                await _productUseCases.Delete(productId);
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
                var productDto = _mapper.Map<ProductViewModel, ProductDto>(productVM);
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
