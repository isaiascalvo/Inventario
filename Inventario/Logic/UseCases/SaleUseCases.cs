using AutoMapper;
using Data;
using Data.Models;
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
    public class SaleUseCases : ISaleUseCases
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICashRepository _cashRepository;
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly IDebitCardRepository _debitCardRepository;
        private readonly IChequeRepository _chequeRepository;
        private readonly IOwnFeesRepository _ownFeesRepository;
        private readonly IFeeRepository _feeRepository;
        private readonly IMapper _mapper;

        public SaleUseCases(ISaleRepository saleRepository,
            IProductRepository productRepository,
            IPriceRepository priceRepository,
            IClientRepository clientRepository,
            ICashRepository cashRepository,
            ICreditCardRepository creditCardRepository,
            IDebitCardRepository debitCardRepository,
            IChequeRepository chequeRepository,
            IOwnFeesRepository ownFeesRepository,
            IFeeRepository feeRepository,
            IMapper mapper
        )
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _priceRepository = priceRepository;
            _clientRepository = clientRepository;
            _cashRepository = cashRepository;
            _creditCardRepository = creditCardRepository;
            _debitCardRepository = debitCardRepository;
            _chequeRepository = chequeRepository;
            _ownFeesRepository = ownFeesRepository;
            _feeRepository = feeRepository;
            _mapper = mapper;
        }

        public async Task<SaleDto> Create(Guid userId, SaleForCreationDto saleForCreationDto)
        {
            var product = await _productRepository.GetById(saleForCreationDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {saleForCreationDto.ProductId} not found.");

            Client client = null;
            if (saleForCreationDto.ClientId.HasValue)
            {
                client = await _clientRepository.GetById(saleForCreationDto.ClientId.Value);
                if (client == null)
                    throw new KeyNotFoundException($"Client with id: {saleForCreationDto.ClientId} not found.");
            }

            var price = (await _priceRepository.GetAll())
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefault(x => x.ProductId == saleForCreationDto.ProductId && x.DateTime <= saleForCreationDto.Date && !x.IsDeleted);

            var sale = new Sale()
            {
                ProductId = saleForCreationDto.ProductId,
                ClientId = saleForCreationDto.ClientId,
                ClientName = client != null ? client.Name + " " + client.Lastname : saleForCreationDto.ClientName,
                Date = saleForCreationDto.Date.ToLocalTime(),
                Quantity = saleForCreationDto.Quantity,
                //Amount = price.Value * saleForCreationDto.Quantity,
                CreatedBy = userId
            };
            product.Stock -= sale.Quantity;

            sale = await _saleRepository.Add(sale);

            Payment payment = null;

            switch (saleForCreationDto.PaymentType)
            {
                case Util.Enums.ePaymentTypes.Cash:
                    var cashDto = (CashForCreationDto)saleForCreationDto.Payment;
                    payment = new Cash()
                    {
                        SaleId = sale.Id,
                        Amount = saleForCreationDto.Payment.Amount,
                        Discount = cashDto.Discount,
                        CreatedBy = sale.CreatedBy
                    };
                    await _cashRepository.Add((Cash)payment);
                    break;
                case Util.Enums.ePaymentTypes.OwnFees:
                    var ownFeesDto = (OwnFeesForCreationDto)saleForCreationDto.Payment;
                    payment = new OwnFees(ownFeesDto.ExpirationDate, sale.CreatedBy)
                    {
                        SaleId = sale.Id,
                        Amount = saleForCreationDto.Payment.Amount,
                        Quantity = ownFeesDto.Quantity,
                        CreatedBy = sale.CreatedBy
                    };

                    payment = await _ownFeesRepository.Add((OwnFees)payment);
                    foreach (var fee in ((OwnFees)payment).FeeList)
                    {
                        await _feeRepository.Add(fee);
                    }
                    break;
                case Util.Enums.ePaymentTypes.CreditCard:
                    var creditCardDto = (CreditCardForCreationDto)saleForCreationDto.Payment;
                    payment = new CreditCard()
                    {
                        SaleId = sale.Id,
                        Amount = saleForCreationDto.Payment.Amount,
                        CardType = creditCardDto.CardType,
                        Bank = creditCardDto.Bank,
                        Discount = creditCardDto.Discount,
                        CreatedBy = sale.CreatedBy
                    };
                    await _creditCardRepository.Add((CreditCard)payment);
                    break;
                case Util.Enums.ePaymentTypes.DebitCard:
                    var debitCardDto = (DebitCardForCreationDto)saleForCreationDto.Payment;
                    payment = new DebitCard()
                    {
                        SaleId = sale.Id,
                        Amount = saleForCreationDto.Payment.Amount,
                        CardType = debitCardDto.CardType,
                        Bank = debitCardDto.Bank,
                        Surcharge = debitCardDto.Surcharge,
                        CreatedBy = sale.CreatedBy
                    };
                    await _debitCardRepository.Add((DebitCard)payment);
                    break;
                case Util.Enums.ePaymentTypes.Cheque:
                    var chequeDto = (ChequeForCreationDto)saleForCreationDto.Payment;
                    payment = new Cheque()
                    {
                        SaleId = sale.Id,
                        Amount = saleForCreationDto.Payment.Amount,
                        Nro = chequeDto.Nro,
                        Bank = chequeDto.Bank,
                        CreatedBy = sale.CreatedBy
                    };
                    await _chequeRepository.Add((Cheque)payment);
                    break;
                default:
                    break;
            }

            sale.Payment = payment;

            await _productRepository.Update(product);
            await _saleRepository.CommitAsync();
            await _productRepository.CommitAsync();
            return _mapper.Map<Sale, SaleDto>(sale);
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var sale = await _saleRepository.Delete(userId, id);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with id: {id} not found.");

            var product = await _productRepository.GetById(sale.ProductId);
            if (product == null)
                product = (await _productRepository.FindDeleted(x => x.Id == sale.ProductId)).FirstOrDefault();
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {sale.ProductId} not found.");

            product.Stock += sale.Quantity;
            await _productRepository.Update(product);

            await _saleRepository.CommitAsync();
            await _productRepository.CommitAsync();

            //deletear payments
        }

        public async Task<IEnumerable<SaleDto>> GetAll()
        {
            var sale = (await _saleRepository.GetAll(x => x.Product, x => x.Client, x => x.Payment)).OrderByDescending(x => x.Date);
            var saleDto = _mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDto>>(sale);
            return saleDto;
        }

        //public async Task<IEnumerable<SaleDto>> GetByFilters(SaleFiltersDto filters)
        //{
        //    var tt = filters.GetExpresion();
        //    var sales = (await _saleRepository.GetAll()).AsQueryable().Where(tt).ToList().OrderBy(x => x.Name);
        //    return _mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDto>>(sales);
        //}


        public async Task<SaleDto> GetOne(Guid id)
        {
            var sale = await _saleRepository.GetById(id, x => x.Payment);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with id: {id} not found.");

            return _mapper.Map<Sale, SaleDto>(sale);
        }

        public async Task Update(Guid id, SaleDto saleDto)
        {
            var newProduct = await _productRepository.GetById(saleDto.ProductId);
            if (newProduct == null)
                throw new KeyNotFoundException($"Product with id: {saleDto.ProductId} not found.");

            Client client = null;
            if (saleDto.ClientId.HasValue)
            {
                client = await _clientRepository.GetById(saleDto.ClientId.Value);
                if (client == null)
                    throw new KeyNotFoundException($"Client with id: {saleDto.ClientId} not found.");
            }

            var sale = await _saleRepository.GetById(id);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with id: {id} not found.");

            var price = (await _priceRepository.GetAll())
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefault(x => x.ProductId == saleDto.ProductId && x.DateTime <= saleDto.Date && !x.IsDeleted);

            if (saleDto.ProductId != sale.ProductId)
            {
                var oldProduct = await _productRepository.GetById(sale.ProductId);
                if (oldProduct == null)
                    throw new KeyNotFoundException($"Product with id: {sale.ProductId} not found.");

                oldProduct.Stock += sale.Quantity;
                newProduct.Stock -= saleDto.Quantity;
                await _productRepository.Update(oldProduct);
                await _productRepository.Update(newProduct);
            }
            else
            {
                newProduct.Stock += sale.Quantity - saleDto.Quantity;
                await _productRepository.Update(newProduct);
            }

            sale.ProductId = saleDto.ProductId;
            sale.ClientId = saleDto.ClientId;
            sale.ClientName = client != null ? client.Name + " " + client.Lastname : saleDto.ClientName;
            sale.Date = saleDto.Date.ToLocalTime();
            sale.Quantity = saleDto.Quantity;
            //sale.Amount = price.Value * saleDto.Quantity;
            sale.LastModificationBy = saleDto.LastModificationBy;

            await _saleRepository.Update(sale);
            await _productRepository.CommitAsync();
            await _saleRepository.CommitAsync();
        }
    }
}
