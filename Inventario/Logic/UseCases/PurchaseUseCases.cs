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
    public class PurchaseUseCases : IPurchaseUseCases
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public PurchaseUseCases(IPurchaseRepository purchaseRepository, IProductRepository productRepository, IPriceRepository priceRepository, IClientRepository clientRepository, IMapper mapper)
        {
            _purchaseRepository = purchaseRepository;
            _productRepository = productRepository;
            _priceRepository = priceRepository;
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<PurchaseDto> Create(Guid userId, PurchaseForCreationDto purchaseForCreationDto)
        {
            var product = await _productRepository.GetById(purchaseForCreationDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {purchaseForCreationDto.ProductId} not found.");

            Client client = null;
            if (purchaseForCreationDto.ClientId.HasValue)
            {
                client = await _clientRepository.GetById(purchaseForCreationDto.ClientId.Value);
                if (client == null)
                    throw new KeyNotFoundException($"Client with id: {purchaseForCreationDto.ClientId} not found.");
            }

            var price = (await _priceRepository.GetAll())
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefault(x => x.ProductId == purchaseForCreationDto.ProductId && x.DateTime <= purchaseForCreationDto.Date && !x.IsDeleted);

            var purchase = new Purchase()
            {
                ProductId = purchaseForCreationDto.ProductId,
                ClientId = purchaseForCreationDto.ClientId,
                ClientName = client != null ? client.Name + " " + client.Lastname : purchaseForCreationDto.ClientName,
                Date = purchaseForCreationDto.Date,
                Quantity = purchaseForCreationDto.Quantity,
                Amount = price.Value * purchaseForCreationDto.Quantity,
                CreatedBy = userId
            };
            product.Stock -= purchase.Quantity;

            purchase = await _purchaseRepository.Add(purchase);
            await _productRepository.Update(product);
            await _purchaseRepository.CommitAsync();
            await _productRepository.CommitAsync();
            return _mapper.Map<Purchase, PurchaseDto>(purchase);
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var purchase = await _purchaseRepository.Delete(userId, id);
            if (purchase == null)
                throw new KeyNotFoundException($"Purchase with id: {id} not found.");

            var product = await _productRepository.GetById(purchase.ProductId);
            if (product == null)
                product = (await _productRepository.FindDeleted(x => x.Id == purchase.ProductId)).FirstOrDefault();
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {purchase.ProductId} not found.");

            product.Stock += purchase.Quantity;
            await _productRepository.Update(product);

            await _purchaseRepository.CommitAsync();
            await _productRepository.CommitAsync();
        }

        public async Task<IEnumerable<PurchaseDto>> GetAll()
        {
            var purchase = await _purchaseRepository.GetAll(x => x.Product, x => x.Client);
            var purchaseDto = _mapper.Map<IEnumerable<Purchase>, IEnumerable<PurchaseDto>>(purchase);
            return purchaseDto;
        }

        //public async Task<IEnumerable<PurchaseDto>> GetByFilters(PurchaseFiltersDto filters)
        //{
        //    var tt = filters.GetExpresion();
        //    var purchases = (await _purchaseRepository.GetAll()).AsQueryable().Where(tt).ToList().OrderBy(x => x.Name);
        //    return _mapper.Map<IEnumerable<Purchase>, IEnumerable<PurchaseDto>>(purchases);
        //}


        public async Task<PurchaseDto> GetOne(Guid id)
        {
            var purchase = await _purchaseRepository.GetById(id);
            if (purchase == null)
                throw new KeyNotFoundException($"Purchase with id: {id} not found.");

            return _mapper.Map<Purchase, PurchaseDto>(purchase);
        }

        public async Task Update(Guid id, PurchaseDto purchaseDto)
        {
            var newProduct = await _productRepository.GetById(purchaseDto.ProductId);
            if (newProduct == null)
                throw new KeyNotFoundException($"Product with id: {purchaseDto.ProductId} not found.");

            Client client = null;
            if (purchaseDto.ClientId.HasValue)
            {
                client = await _clientRepository.GetById(purchaseDto.ClientId.Value);
                if (client == null)
                    throw new KeyNotFoundException($"Client with id: {purchaseDto.ClientId} not found.");
            }

            var purchase = await _purchaseRepository.GetById(id);
            if (purchase == null)
                throw new KeyNotFoundException($"Purchase with id: {id} not found.");

            var price = (await _priceRepository.GetAll())
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefault(x => x.ProductId == purchaseDto.ProductId && x.DateTime <= purchaseDto.Date && !x.IsDeleted);

            if (purchaseDto.ProductId != purchase.ProductId)
            {
                var oldProduct = await _productRepository.GetById(purchase.ProductId);
                if (oldProduct == null)
                    throw new KeyNotFoundException($"Product with id: {purchase.ProductId} not found.");

                oldProduct.Stock += purchase.Quantity;
                newProduct.Stock -= purchaseDto.Quantity;
                await _productRepository.Update(oldProduct);
                await _productRepository.Update(newProduct);
            }
            else
            {
                newProduct.Stock += purchase.Quantity - purchaseDto.Quantity;
                await _productRepository.Update(newProduct);
            }

            purchase.ProductId = purchaseDto.ProductId;
            purchase.ClientId = purchaseDto.ClientId;
            purchase.ClientName = client != null ? client.Name + " " + client.Lastname : purchaseDto.ClientName;
            purchase.Date = purchaseDto.Date;
            purchase.Quantity = purchaseDto.Quantity;
            purchase.Amount = price.Value * purchaseDto.Quantity;
            purchase.LastModificationBy = purchaseDto.LastModificationBy;

            await _purchaseRepository.Update(purchase);
            await _productRepository.CommitAsync();
            await _purchaseRepository.CommitAsync();
        }
    }
}
