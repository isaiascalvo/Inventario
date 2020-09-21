using AutoMapper;
using Data;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ProductEntryLineUseCases : IProductEntryLineUseCases
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductEntryRepository _productEntryRepository;
        private readonly IProductEntryLineRepository _productEntryLineRepository;
        private readonly IMapper _mapper;

        public ProductEntryLineUseCases(IProductRepository productRepository, IProductEntryRepository productEntryRepository, IProductEntryLineRepository productEntryLineRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _productEntryRepository = productEntryRepository;
            _productEntryLineRepository = productEntryLineRepository;
            _mapper = mapper;
        }
        public async Task<ProductEntryLineDto> Create(Guid userId, ProductEntryLineForCreationDto productEntryLineForCreationDto)
        {
            var product = await _productRepository.GetById(productEntryLineForCreationDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {productEntryLineForCreationDto.ProductId} not found.");

            var productEntry = await _productEntryRepository.GetById(productEntryLineForCreationDto.ProductEntryId.Value);
            if (productEntry == null)
                throw new KeyNotFoundException($"Product Entry with id: {productEntryLineForCreationDto.ProductEntryId.Value} not found.");

            var productEntryLine = new ProductEntryLine()
            {
                Quantity = productEntryLineForCreationDto.Quantity,
                ProductId = productEntryLineForCreationDto.ProductId,
                ProductEntryId = productEntryLineForCreationDto.ProductEntryId.Value,
                CreatedBy = userId
            };

            productEntryLine = await _productEntryLineRepository.Add(productEntryLine);

            product.Stock += productEntryLineForCreationDto.Quantity;
            await _productRepository.Update(product);
        
            await _productEntryLineRepository.CommitAsync();
            await _productRepository.CommitAsync();

            return _mapper.Map<ProductEntryLine, ProductEntryLineDto>(productEntryLine);
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var productEntryLine = await _productEntryLineRepository.GetById(id, x => x.Include(pel => pel.ProductEntry));
            if (productEntryLine == null)
                throw new KeyNotFoundException($"Product entry line with id: {id} not found.");

            var product = await _productRepository.GetById(productEntryLine.ProductId);
            if (product == null)
                product = (await _productRepository.FindDeleted(x => x.Id == productEntryLine.ProductId)).FirstOrDefault();
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {productEntryLine.ProductId} not found.");

            product.Stock -= productEntryLine.Quantity * (productEntryLine.ProductEntry.IsEntry ? 1 : -1);
            await _productRepository.Update(product);
            await _productEntryLineRepository.Delete(userId, productEntryLine.Id);
            
            await _productRepository.CommitAsync();
            await _productEntryLineRepository.CommitAsync();
        }

        public async Task<IEnumerable<ProductEntryLineDto>> GetAll()
        {
            var productEntryLines = await _productEntryLineRepository.GetAll();
            return _mapper.Map<IEnumerable<ProductEntryLine>, IEnumerable<ProductEntryLineDto>>(productEntryLines);
        }

        public async Task<IEnumerable<ProductEntryLineDto>> GetByProductEntry(Guid productEntryId)
        {
            var productEntryLines = (await _productEntryLineRepository.GetAll()).Where(x => x.ProductEntryId == productEntryId);
            return _mapper.Map<IEnumerable<ProductEntryLine>, IEnumerable<ProductEntryLineDto>>(productEntryLines);
        }

        public async Task<ProductEntryLineDto> GetOne(Guid id)
        {
            var productEntryLine = await _productEntryLineRepository.GetById(id);
            if (productEntryLine == null)
                throw new KeyNotFoundException($"Product entry line with id: {id} not found.");

            return _mapper.Map<ProductEntryLine, ProductEntryLineDto>(productEntryLine);
        }

        public async Task Update(Guid id, ProductEntryLineDto productEntryLineDto)
        {
            var productEntryLine = await _productEntryLineRepository.GetById(id);
            if (productEntryLine == null)
                throw new KeyNotFoundException($"Product entry line with id: {id} not found.");

            productEntryLine.ProductId = productEntryLineDto.ProductId;
            productEntryLine.Quantity = productEntryLineDto.Quantity;
            productEntryLine.LastModificationBy = productEntryLineDto.LastModificationBy;

            await _productEntryLineRepository.Update(productEntryLine);
            await _productEntryLineRepository.CommitAsync();
        }
    }
}
