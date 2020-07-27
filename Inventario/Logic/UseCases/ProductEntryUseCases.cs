using AutoMapper;
using Data;
using Infrastructure.Repositories;
using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ProductEntryUseCases : IProductEntryUseCases
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductEntryRepository _productEntryRepository;
        private readonly IProductEntryLineRepository _productEntryLineRepository;
        private readonly IMapper _mapper;

        public ProductEntryUseCases(IProductRepository productRepository, IProductEntryRepository productEntryRepository, IProductEntryLineRepository productEntryLineRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _productEntryRepository = productEntryRepository;
            _productEntryLineRepository = productEntryLineRepository;
            _mapper = mapper;
        }

        public async Task<ProductEntryDto> Create(Guid userId, ProductEntryForCreationDto productEntryForCreationDto)
        {
            ProductEntry productEntry = new ProductEntry()
            {
                Date = productEntryForCreationDto.Date,
                CreatedBy = userId
            };

            productEntry = await _productEntryRepository.Add(productEntry);

            //ProductEntryLines
            foreach (var pelDto in productEntryForCreationDto.ProductEntryLines)
            {
                var product = await _productRepository.GetById(pelDto.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product with id: {pelDto.ProductId} not found.");

                var productEntryLine = new ProductEntryLine()
                {
                    Quantity = pelDto.Quantity,
                    ProductId = pelDto.ProductId,
                    ProductEntryId = productEntry.Id,
                    CreatedBy = userId
                };

                productEntryLine = await _productEntryLineRepository.Add(productEntryLine);
                productEntry.ProductEntryLines.Add(productEntryLine);
            }

            await _productEntryRepository.CommitAsync();
            await _productEntryLineRepository.CommitAsync();

            var productEntryDto = _mapper.Map<ProductEntry, ProductEntryDto>(productEntry);

            return productEntryDto;
        }

        public Task Delete(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductEntryDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ProductEntryDto> GetOne(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid id, ProductEntryDto productEntryDto)
        {
            throw new NotImplementedException();
        }
    }
}
