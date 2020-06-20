using AutoMapper;
using Data;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ProductUseCases : IProductUseCases
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IMapper _mapper;

        public ProductUseCases(IProductRepository productRepository, ICategoryRepository categoryRepository, IPriceRepository priceRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _priceRepository = priceRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Create(ProductForCreationDto productForCreationDto)
        {
            var product = new Product()
            {
                Name = productForCreationDto.Name,
                Description = productForCreationDto.Description,
                Code = productForCreationDto.Code,
                CategoryId = productForCreationDto.CategoryId,
                VendorId = productForCreationDto.VendorId,
                Brand = productForCreationDto.Brand,
                Active = productForCreationDto.Active,
                Available = productForCreationDto.Available
            };


            //Price

            return _mapper.Map<Product, ProductDto>(await _productRepository.Add(product));
        }

        public async Task Delete(Guid id)
        {
            var product = await _productRepository.Delete(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {id} not found.");
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var products = (await _productRepository.GetAll()).OrderBy(x => x.Name);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            foreach (var prodDto in productsDto)
            {
                var price = (await _priceRepository.GetAll()).Where(p => p.ProductId == prodDto.Id).OrderByDescending(x => x.DateTime).FirstOrDefault();            
                prodDto.Price = _mapper.Map<Price, PriceDto>(price);
            }
            return productsDto;
        }

        public async Task<IEnumerable<ProductDto>> GetByCategory(Guid categoryId)
        {
            var products = (await _productRepository.GetAll()).Where(x => x.CategoryId == categoryId).OrderBy(x => x.Name);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            foreach (var prodDto in productsDto)
            {
                var price = (await _priceRepository.GetAll()).Where(p => p.ProductId == prodDto.Id).OrderByDescending(x => x.DateTime).FirstOrDefault();
                prodDto.Price = _mapper.Map<Price, PriceDto>(price);
            }

            return productsDto;
        }

        public async Task<ProductDto> GetOne(Guid id)
        {
            var product = await _productRepository.GetById(id, p => p.Category, p => p.Vendor);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {id} not found.");

            var productDto = _mapper.Map<Product, ProductDto>(product);

            var price= (await _priceRepository.GetAll()).Where(p => p.ProductId == id).OrderByDescending(x => x.DateTime).FirstOrDefault();

            productDto.Price = _mapper.Map<Price, PriceDto>(price);

            return productDto;
        }

        public async Task Update(Guid id, ProductDto productDto)
        {
            var product = await _productRepository.GetById(id);
            product.Name = productDto.Name;
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Code = productDto.Code;
            product.CategoryId = productDto.CategoryId;
            product.VendorId = productDto.VendorId;
            product.Brand = productDto.Brand;
            product.Active = productDto.Active;
            product.Available = productDto.Available;
            await _productRepository.Update(product);

            //Price
        }
    }
}
