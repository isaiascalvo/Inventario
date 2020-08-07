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
        private readonly IVendorRepository _vendorRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IMapper _mapper;

        public ProductUseCases(IProductRepository productRepository, ICategoryRepository categoryRepository, IVendorRepository vendorRepository, IPriceRepository priceRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _vendorRepository = vendorRepository;
            _priceRepository = priceRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Create(Guid userId, ProductForCreationDto productForCreationDto)
        {
            var vendor = await _vendorRepository.GetById(productForCreationDto.VendorId);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with id: {productForCreationDto.VendorId} not found.");

            var category = await _categoryRepository.GetById(productForCreationDto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with id: {productForCreationDto.CategoryId} not found.");

            var product = new Product()
            {
                Name = productForCreationDto.Name,
                Description = productForCreationDto.Description,
                Code = productForCreationDto.Code,
                CategoryId = productForCreationDto.CategoryId,
                VendorId = productForCreationDto.VendorId,
                Brand = productForCreationDto.Brand,
                Stock = 0,
                UnitOfMeasurement = productForCreationDto.UnitOfMeasurement,
                CreatedBy = userId
            };

            product = await _productRepository.Add(product);

            //Price
            var price = new Price()
            {
                Value = productForCreationDto.Price.Value,
                DateTime = DateTime.Now,
                ProductId = product.Id
            };

            await _priceRepository.Add(price);

            await _productRepository.CommitAsync();
            await _priceRepository.CommitAsync();

            var productDto = _mapper.Map<Product, ProductDto>(product);

            productDto.Price = _mapper.Map<Price, PriceDto>(price);

            return productDto;
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var product = await _productRepository.Delete(userId, id);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {id} not found.");

            await _productRepository.CommitAsync();
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

        public async Task<int> GetTotalQty()
        {
            return (await _productRepository.GetAll()).Count();
        }

        public async Task<IEnumerable<ProductDto>> GetByPageAndQty(int skip, int qty)
        {
            var products = (await _productRepository.GetAll()).OrderBy(x => x.Name).Skip(skip).Take(qty);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            foreach (var prodDto in productsDto)
            {
                var price = (await _priceRepository.GetAll()).Where(p => p.ProductId == prodDto.Id).OrderByDescending(x => x.DateTime).FirstOrDefault();
                prodDto.Price = _mapper.Map<Price, PriceDto>(price);
            }
            return productsDto;
        }
        
        public async Task<IEnumerable<ProductDto>> GetByFilters(ProductFiltersDto filters)
        {
            var tt = filters.GetExpresion();
            var products = (await _productRepository.GetAll()).AsQueryable().Where(tt).ToList().OrderBy(x => x.Name);
            var productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            foreach (var prodDto in productsDto)
            {
                var price = (await _priceRepository.GetAll()).Where(p => p.ProductId == prodDto.Id).OrderByDescending(x => x.DateTime).FirstOrDefault();
                prodDto.Price = _mapper.Map<Price, PriceDto>(price);
            }
            return productsDto;
        }

        public async Task<int> GetTotalQtyByFilters(ProductFiltersDto filters)
        {
            return (await _productRepository.GetAll()).AsQueryable().Where(filters.GetExpresion()).Count();
        }

        public async Task<IEnumerable<ProductDto>> GetFilteredByPageAndQty(ProductFiltersDto filters, int skip, int qty)
        {
            var tt = filters.GetExpresion();
            var products = (await _productRepository.GetAll()).AsQueryable().Where(tt).ToList().OrderBy(x => x.Name).Skip(skip).Take(qty);
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
            var vendor = await _vendorRepository.GetById(productDto.VendorId);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with id: {productDto.VendorId} not found.");

            var category = await _categoryRepository.GetById(productDto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with id: {productDto.CategoryId} not found.");

            var product = await _productRepository.GetById(id);
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Code = productDto.Code;
            product.CategoryId = productDto.CategoryId;
            product.VendorId = productDto.VendorId;
            product.Brand = productDto.Brand;
            product.UnitOfMeasurement = productDto.UnitOfMeasurement;
            product.LastModificationBy = productDto.LastModificationBy;

            await _productRepository.Update(product);

            //Price
            var lastPrice = (await _priceRepository.Find(x => x.ProductId == productDto.Id)).OrderByDescending(x => x.DateTime).FirstOrDefault();
            
            if (lastPrice == null || lastPrice.Value != productDto.Price.Value)
            {
                var price = new Price()
                {
                    Value = productDto.Price.Value,
                    DateTime = DateTime.Now,
                    ProductId = productDto.Id,
                    CreatedBy = productDto.LastModificationBy.Value
                };

                await _priceRepository.Add(price);
            }

            await _productRepository.CommitAsync();
            await _priceRepository.CommitAsync();
        }

        public async Task<double> GetPriceByDate(Guid productId, DateTime date)
        {
            var product = await _productRepository.GetById(productId);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {productId} not found.");

            var price = (await _priceRepository.GetAll())
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefault(x => x.ProductId == productId && x.DateTime <= date);
            if (price == null)
                throw new Exception("Price not found.");

            return price.Value;
        }
    }
}
