﻿using AutoMapper;
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
            try
            {
                ProductEntry productEntry = new ProductEntry()
                {
                    Date = productEntryForCreationDto.Date,
                    IsEntry = productEntryForCreationDto.IsEntry,
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

                    product.Stock += pelDto.Quantity * (productEntry.IsEntry ? 1 : -1);
                    await _productRepository.Update(product);
                }

                await _productEntryRepository.CommitAsync();
                await _productEntryLineRepository.CommitAsync();
                await _productRepository.CommitAsync();

                var productEntryDto = _mapper.Map<ProductEntry, ProductEntryDto>(productEntry);

                return productEntryDto;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var productEntry = _productEntryRepository.AllIncluding(x => x.ProductEntryLines).FirstOrDefault(x => x.Id == id);
            if (productEntry == null)
                throw new KeyNotFoundException($"Product entry with id: {id} not found.");

            productEntry.ProductEntryLines.RemoveAll(x => x.IsDeleted);

            foreach (var pel in productEntry.ProductEntryLines)
            {
                var product = await _productRepository.GetById(pel.ProductId);
                if (product == null)
                    product = (await _productRepository.FindDeleted(x => x.Id == pel.ProductId)).FirstOrDefault();
                if (product == null)
                    throw new KeyNotFoundException($"Product with id: {pel.ProductId} not found.");

                product.Stock -= pel.Quantity * (productEntry.IsEntry ? 1 : -1);
                await _productRepository.Update(product);
                await _productEntryLineRepository.Delete(userId, pel.Id);
            }

            await _productEntryRepository.Delete(userId, id);

            await _productRepository.CommitAsync();
            await _productEntryLineRepository.CommitAsync();
            await _productEntryRepository.CommitAsync();
        }

        public async Task<IEnumerable<ProductEntryDto>> GetAll()
        {
            var productEntries = await _productEntryRepository.GetAll();
            return _mapper.Map<IEnumerable<ProductEntry>, IEnumerable<ProductEntryDto>>(productEntries);
        }

        public async Task<ProductEntryDto> GetOne(Guid id)
        {
            var productEntry = await _productEntryRepository.GetById(id, x => x.ProductEntryLines);
            if (productEntry == null)
                throw new KeyNotFoundException($"Product entry with id: {id} not found.");
            productEntry.ProductEntryLines.RemoveAll(x => x.IsDeleted);
            return _mapper.Map<ProductEntry, ProductEntryDto>(productEntry);
        }

        public async Task Update(Guid id, ProductEntryDto productEntryDto)
        {
            var productEntry = await _productEntryRepository.GetById(productEntryDto.Id, x => x.ProductEntryLines);
            if (productEntry == null)
                throw new KeyNotFoundException($"Product entry with id: {id} not found.");

            //Delete PEL Deleted
            var deleted = productEntry.ProductEntryLines.Where(
                pel => !productEntryDto.ProductEntryLines.Any(pelDto => pelDto.Id == pel.Id)
            );

            foreach (var pel in deleted)
            {
                var product = await _productRepository.GetById(pel.ProductId);
                if (product == null)
                    product = (await _productRepository.FindDeleted(x => x.Id == pel.ProductId)).FirstOrDefault();
                if (product == null)
                    throw new KeyNotFoundException($"Product with id: {pel.ProductId} not found.");

                product.Stock -= pel.Quantity * (productEntry.IsEntry ? 1 : -1);
                await _productRepository.Update(product);
                await _productEntryLineRepository.Delete(productEntryDto.LastModificationBy.Value, pel.Id);
            }

            //Add PEL Added
            var added = productEntryDto.ProductEntryLines.Where(x => x.Id == Guid.Empty);

            foreach (var pel in added)
            {
                var product = await _productRepository.GetById(pel.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product with id: {pel.ProductId} not found.");

                var productEntryLine = new ProductEntryLine()
                {
                    Quantity = pel.Quantity,
                    ProductId = pel.ProductId,
                    ProductEntryId = productEntryDto.Id,
                    CreatedBy = productEntryDto.LastModificationBy.Value
                };

                productEntryLine = await _productEntryLineRepository.Add(productEntryLine);

                product.Stock += pel.Quantity * (productEntryDto.IsEntry ? 1 : -1);
                await _productRepository.Update(product);
            }

            //Change Input/Output
            if (productEntry.IsEntry != productEntryDto.IsEntry)
            {
                var changed = productEntry.ProductEntryLines.Where(pel => !deleted.Contains(pel));
                foreach (var pel in changed)
                {
                    var product = await _productRepository.GetById(pel.ProductId);
                    if (product == null)
                        product = (await _productRepository.FindDeleted(x => x.Id == pel.ProductId)).FirstOrDefault();
                    if (product == null)
                        throw new KeyNotFoundException($"Product with id: {pel.ProductId} not found.");

                    product.Stock += pel.Quantity * 2 * (productEntryDto.IsEntry ? 1 : -1);
                    await _productRepository.Update(product);
                }
            }

            productEntry.Date = productEntryDto.Date;
            productEntry.IsEntry = productEntryDto.IsEntry;
            productEntry.LastModificationBy = productEntryDto.LastModificationBy;
            
            await _productEntryRepository.Update(productEntry);
            await _productEntryRepository.CommitAsync();
            await _productEntryLineRepository.CommitAsync();
            await _productRepository.CommitAsync();
        }
    }
}
