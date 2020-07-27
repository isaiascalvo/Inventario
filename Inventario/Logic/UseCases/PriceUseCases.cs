using AutoMapper;
using Data;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class PriceUseCases: IPriceUseCases
    {
        private readonly IPriceRepository _priceRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public PriceUseCases(IPriceRepository priceRepository, IProductRepository productRepository, IMapper mapper)
        {
            _priceRepository = priceRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PriceDto> Create(Guid userId, PriceForCreationDto priceForCreationDto)
        {
            var product = await _productRepository.GetById(priceForCreationDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {priceForCreationDto.ProductId} not found.");

            var price = new Price()
            {
                Value = priceForCreationDto.Value,
                DateTime = priceForCreationDto.DateTime,
                ProductId = priceForCreationDto.ProductId,
                Product = product,
                CreatedBy = userId
            };

            price = await _priceRepository.Add(price);
            await _priceRepository.CommitAsync();
            return _mapper.Map<Price, PriceDto>(price);
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var price = await _priceRepository.Delete(userId, id);
            if (price == null)
                throw new KeyNotFoundException($"Price with id: {id} not found.");

            await _priceRepository.CommitAsync();
        }

        public IEnumerable<PriceDto> GetAll()
        {
            var prices = _priceRepository.AllIncluding(p => p.Product);
            var pricesDto = _mapper.Map<IEnumerable<Price>, IEnumerable<PriceDto>>(prices);
            return pricesDto;
        }

        public async Task<PriceDto> GetOne(Guid id)
        {
            var price = await _priceRepository.GetById(id);
            if (price == null)
                throw new KeyNotFoundException($"Price with id: {id} not found.");

            return _mapper.Map<Price, PriceDto>(price);
        }

        public async Task Update(Guid id, PriceDto priceDto)
        {
            var price = await _priceRepository.GetById(id);
            if (price == null)
                throw new KeyNotFoundException($"Price with id: {id} not found.");

            var product = await _productRepository.GetById(priceDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {priceDto.ProductId} not found.");

            price.Value = priceDto.Value;
            price.ProductId = priceDto.ProductId;
            price.Product = product;
            price.DateTime = priceDto.DateTime;
            price.LastModificationBy = priceDto.LastModificationBy;

            await _priceRepository.Update(price);
            await _priceRepository.CommitAsync();
        }
    }
}
