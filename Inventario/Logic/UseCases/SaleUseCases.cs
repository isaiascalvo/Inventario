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
        private readonly IChequesPaymentRepository _chequesPaymentRepository;
        private readonly IChequeRepository _chequeRepository;
        private readonly IOwnFeesRepository _ownFeesRepository;
        private readonly IFeeRepository _feeRepository;
        private readonly IFeeRuleRepository _feeRuleRepository;
        private readonly IMapper _mapper;

        public SaleUseCases(ISaleRepository saleRepository,
            IProductRepository productRepository,
            IPriceRepository priceRepository,
            IClientRepository clientRepository,
            ICashRepository cashRepository,
            ICreditCardRepository creditCardRepository,
            IDebitCardRepository debitCardRepository,
            IChequesPaymentRepository chequesPaymentRepository,
            IChequeRepository chequeRepository,
            IOwnFeesRepository ownFeesRepository,
            IFeeRepository feeRepository,
            IFeeRuleRepository feeRuleRepository,
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
            _chequesPaymentRepository = chequesPaymentRepository;
            _chequeRepository = chequeRepository;
            _ownFeesRepository = ownFeesRepository;
            _feeRepository = feeRepository;
            _feeRuleRepository = feeRuleRepository;
            _mapper = mapper;
        }

        public async Task<SaleDto> Create(Guid userId, SaleForCreationDto saletForCreationDto)
        {
            try
            {

            
                var product = await _productRepository.GetById(saletForCreationDto.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product with id: {saletForCreationDto.ProductId} not found.");

                Client client = null;
                if (saletForCreationDto.ClientId.HasValue)
                {
                    client = await _clientRepository.GetById(saletForCreationDto.ClientId.Value);
                    if (client == null)
                        throw new KeyNotFoundException($"Client with id: {saletForCreationDto.ClientId} not found.");
                }

                var price = (await _priceRepository.GetAll())
                    .OrderByDescending(x => x.DateTime)
                    .FirstOrDefault(
                        x => x.ProductId == saletForCreationDto.ProductId &&
                        x.DateTime.ToLocalTime() <= saletForCreationDto.Date.ToLocalTime() &&
                        !x.IsDeleted
                    );

                var sale = new Sale()
                {
                    ProductId = saletForCreationDto.ProductId,
                    ClientId = saletForCreationDto.ClientId,
                    ClientName = client != null ? client.Name + " " + client.Lastname : saletForCreationDto.ClientName,
                    Date = saletForCreationDto.Date.ToLocalTime(),
                    Quantity = saletForCreationDto.Quantity,
                    PaymentType = saletForCreationDto.PaymentType,
                    CreatedBy = userId
                };
                product.Stock -= sale.Quantity;

                sale = await _saleRepository.Add(sale);

                Payment payment = null;
                double amount;
                switch (saletForCreationDto.PaymentType)
                {
                    case Util.Enums.ePaymentTypes.Cash:
                        var cashDto = (CashForCreationDto)saletForCreationDto.Cash;
                        payment = new Cash(price.Value * saletForCreationDto.Quantity, cashDto.Discount)
                        {
                            SaleId = sale.Id,
                            CreatedBy = sale.CreatedBy
                        };

                        await _cashRepository.Add((Cash)payment);
                        break;
                    case Util.Enums.ePaymentTypes.OwnFees:
                        var ownFeesDto = (OwnFeesForCreationDto)saletForCreationDto.OwnFees;
                        FeeRule feeRule = null;
                        if (ownFeesDto.FeeRuleId.HasValue)
                        {
                            feeRule = await _feeRuleRepository.GetById(ownFeesDto.FeeRuleId.Value);
                            if (feeRule == null)
                                throw new KeyNotFoundException($"Fee Rule with id: {ownFeesDto.FeeRuleId.Value} not found.");
                        }

                        amount = price.Value * saletForCreationDto.Quantity;
                        if (feeRule != null)
                        {
                            double percentage = feeRule.Percentage * ownFeesDto.Quantity / 100;
                            amount *= (1 + percentage);
                        }

                        payment = new OwnFees(ownFeesDto.ExpirationDate, amount, ownFeesDto.Quantity, sale.CreatedBy)
                        {
                            SaleId = sale.Id,
                            FeeRuleId = ownFeesDto.FeeRuleId
                        };

                        payment = await _ownFeesRepository.Add((OwnFees)payment);
                        foreach (var fee in ((OwnFees)payment).FeeList)
                        {
                            await _feeRepository.Add(fee);
                        }
                        break;
                    case Util.Enums.ePaymentTypes.CreditCard:
                        var creditCardDto = (CreditCardForCreationDto)saletForCreationDto.CreditCard;
                        payment = new CreditCard(price.Value * saletForCreationDto.Quantity, creditCardDto.Discount)
                        {
                            SaleId = sale.Id,
                            CardType = creditCardDto.CardType,
                            Bank = creditCardDto.Bank,
                            CreatedBy = sale.CreatedBy
                        };

                        await _creditCardRepository.Add((CreditCard)payment);
                        break;
                    case Util.Enums.ePaymentTypes.DebitCard:
                        var debitCardDto = (DebitCardForCreationDto)saletForCreationDto.DebitCard;
                        payment = new DebitCard(price.Value * saletForCreationDto.Quantity, debitCardDto.Surcharge)
                        {
                            SaleId = sale.Id,
                            CardType = debitCardDto.CardType,
                            Bank = debitCardDto.Bank,
                            CreatedBy = sale.CreatedBy
                        };

                        await _debitCardRepository.Add((DebitCard)payment);
                        break;
                    case Util.Enums.ePaymentTypes.Cheques:
                        var chequesDto = (ChequesPaymentForCreationDto)saletForCreationDto.Cheques;
                        amount = price.Value * saletForCreationDto.Quantity;

                        if (chequesDto.ListOfCheques.Sum(x => x.Value) != amount)
                            throw new InvalidOperationException("The sum of cheques list is different of amount.");
                        

                        payment = new ChequesPayment()
                        {
                            SaleId = sale.Id,
                            Amount = amount,
                            CreatedBy = sale.CreatedBy
                        };

                        await _chequesPaymentRepository.Add((ChequesPayment)payment);

                        foreach (var c in chequesDto.ListOfCheques)
                        {
                            var cheque = new Cheque()
                            {
                                ChequesPaymentId = payment.Id,
                                Bank = c.Bank,
                                Nro = c.Nro,
                                Value = c.Value,
                                CreatedBy = sale.CreatedBy
                            };
                        
                            await _chequeRepository.Add(cheque);
                        }
                        break;
                    default:
                        break;
                }

                sale.Payment = payment;

                await _productRepository.Update(product);
                await _saleRepository.CommitAsync();
                return _mapper.Map<Sale, SaleDto>(sale);
            }
            catch (Exception e)
            {
                throw e;
            }
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
            try
            {
                var sales = (await _saleRepository.GetAll(x => x.Product, x => x.Client, x => x.Payment)).OrderByDescending(x => x.Date);
                var salesDto = _mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDto>>(sales);
                //foreach (var s in salesDto)
                //{
                //    switch (s.PaymentType)
                //    {
                //        case Util.Enums.ePaymentTypes.Cash:
                //            s.Cash = _mapper.Map<Cash, CashDto>((Cash)sales.FirstOrDefault(x => x.Id == s.Id).Payment);
                //            break;
                //        case Util.Enums.ePaymentTypes.OwnFees:
                //            s.OwnFees = _mapper.Map<OwnFees, OwnFeesDto>((OwnFees)sales.FirstOrDefault(x => x.Id == s.Id).Payment);
                //            break;
                //        case Util.Enums.ePaymentTypes.CreditCard:
                //            s.CreditCard = _mapper.Map<CreditCard, CreditCardDto>((CreditCard)sales.FirstOrDefault(x => x.Id == s.Id).Payment);
                //            break;
                //        case Util.Enums.ePaymentTypes.DebitCard:
                //            s.DebitCard = _mapper.Map<DebitCard, DebitCardDto>((DebitCard)sales.FirstOrDefault(x => x.Id == s.Id).Payment);
                //            break;
                //        case Util.Enums.ePaymentTypes.Cheques:
                //            s.Cheques = _mapper.Map<ChequesPayment, ChequesPaymentDto>((ChequesPayment)sales.FirstOrDefault(x => x.Id == s.Id).Payment);
                //            s.Cheques.ListOfCheques =
                //                _mapper.Map<IEnumerable<Cheque>, IEnumerable<ChequeDto>>(
                //                    (await _chequeRepository.GetAll()).Where(x => x.ChequesPaymentId == s.Cheques.Id)
                //                ).ToList();
                //            break;
                //        default:
                //            break;
                //    }
                //}
                //return salesDto;
                return await SetSalePayments(sales, salesDto);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> GetTotalQty()
        {
            return (await _saleRepository.GetAll()).Count();
        }

        public async Task<int> GetTotalQtyByFilters(SaleFiltersDto filtersDto)
        {
            return (await _saleRepository.GetAll()).AsQueryable().Where(filtersDto.GetExpresion()).Count();
        }

        public async Task<IEnumerable<SaleDto>> GetByPageAndQty(int skip, int qty)
        {
            var sales = (await _saleRepository.GetAll(x => x.Product, x => x.Client, x => x.Payment)).OrderByDescending(x => x.Date).Skip(skip).Take(qty);
            var salesDto = _mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDto>>(sales);
            return await SetSalePayments(sales, salesDto);
        }

        public async Task<IEnumerable<SaleDto>> GetFilteredByPageAndQty(SaleFiltersDto filtersDto, int skip, int qty)
        {
            var sales = (await _saleRepository.GetAll(x => x.Product, x => x.Client, x => x.Payment))
                .AsQueryable().Where(filtersDto.GetExpresion()).ToList().OrderByDescending(x => x.Date).Skip(skip).Take(qty);
            var salesDto = _mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDto>>(sales);
            return await SetSalePayments(sales, salesDto);
        }

        public async Task<SaleDto> GetOne(Guid id)
        {
            var sale = await _saleRepository.GetById(id, x => x.Payment);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with id: {id} not found.");

            var saleDto = _mapper.Map<Sale, SaleDto>(sale);
            switch (saleDto.PaymentType)
            {
                case Util.Enums.ePaymentTypes.Cash:
                    saleDto.Cash = _mapper.Map<Cash, CashDto>((Cash)sale.Payment);
                    break;
                case Util.Enums.ePaymentTypes.OwnFees:
                    saleDto.OwnFees = _mapper.Map<OwnFees, OwnFeesDto>((OwnFees)sale.Payment);
                    break;
                case Util.Enums.ePaymentTypes.CreditCard:
                    saleDto.CreditCard = _mapper.Map<CreditCard, CreditCardDto>((CreditCard)sale.Payment);
                    break;
                case Util.Enums.ePaymentTypes.DebitCard:
                    saleDto.DebitCard = _mapper.Map<DebitCard, DebitCardDto>((DebitCard)sale.Payment);
                    break;
                case Util.Enums.ePaymentTypes.Cheques:
                    saleDto.Cheques = _mapper.Map<ChequesPayment, ChequesPaymentDto>((ChequesPayment)sale.Payment);
                    saleDto.Cheques.ListOfCheques =
                                _mapper.Map<IEnumerable<Cheque>, IEnumerable<ChequeDto>>(
                                    (await _chequeRepository.GetAll()).Where(x => x.ChequesPaymentId == saleDto.Cheques.Id)
                                ).ToList();
                    break;
                default:
                    break;
            }
            return saleDto;
        }


        //public async Task Update(Guid id, SaleDto saleDto)
        //{
        //    var newProduct = await _productRepository.GetById(saleDto.ProductId);
        //    if (newProduct == null)
        //        throw new KeyNotFoundException($"Product with id: {saleDto.ProductId} not found.");

        //    Client client = null;
        //    if (saleDto.ClientId.HasValue)
        //    {
        //        client = await _clientRepository.GetById(saleDto.ClientId.Value);
        //        if (client == null)
        //            throw new KeyNotFoundException($"Client with id: {saleDto.ClientId} not found.");
        //    }

        //    var sale = await _saleRepository.GetById(id);
        //    if (sale == null)
        //        throw new KeyNotFoundException($"Sale with id: {id} not found.");

        //    var price = (await _priceRepository.GetAll())
        //        .OrderByDescending(x => x.DateTime)
        //        .FirstOrDefault(x => x.ProductId == saleDto.ProductId && x.DateTime <= saleDto.Date && !x.IsDeleted);

        //    if (saleDto.ProductId != sale.ProductId)
        //    {
        //        var oldProduct = await _productRepository.GetById(sale.ProductId);
        //        if (oldProduct == null)
        //            throw new KeyNotFoundException($"Product with id: {sale.ProductId} not found.");

        //        oldProduct.Stock += sale.Quantity;
        //        newProduct.Stock -= saleDto.Quantity;
        //        await _productRepository.Update(oldProduct);
        //        await _productRepository.Update(newProduct);
        //    }
        //    else
        //    {
        //        newProduct.Stock += sale.Quantity - saleDto.Quantity;
        //        await _productRepository.Update(newProduct);
        //    }

        //    sale.ProductId = saleDto.ProductId;
        //    sale.ClientId = saleDto.ClientId;
        //    sale.ClientName = client != null ? client.Name + " " + client.Lastname : saleDto.ClientName;
        //    sale.Date = saleDto.Date.ToLocalTime();
        //    sale.Quantity = saleDto.Quantity;
        //    //sale.Amount = price.Value * saleDto.Quantity;
        //    sale.LastModificationBy = saleDto.LastModificationBy;

        //    await _saleRepository.Update(sale);
        //    await _productRepository.CommitAsync();
        //    await _saleRepository.CommitAsync();
        //}

        private async Task<IEnumerable<SaleDto>> SetSalePayments(IEnumerable<Sale> sales, IEnumerable<SaleDto> salesDto)
        {
            foreach (var s in salesDto)
            {
                switch (s.PaymentType)
                {
                    case Util.Enums.ePaymentTypes.Cash:
                        s.Cash = _mapper.Map<Cash, CashDto>((Cash)sales.FirstOrDefault(x => x.Id == s.Id).Payment);
                        break;
                    case Util.Enums.ePaymentTypes.OwnFees:
                        s.OwnFees = _mapper.Map<OwnFees, OwnFeesDto>((OwnFees)sales.FirstOrDefault(x => x.Id == s.Id).Payment);
                        break;
                    case Util.Enums.ePaymentTypes.CreditCard:
                        s.CreditCard = _mapper.Map<CreditCard, CreditCardDto>((CreditCard)sales.FirstOrDefault(x => x.Id == s.Id).Payment);
                        break;
                    case Util.Enums.ePaymentTypes.DebitCard:
                        s.DebitCard = _mapper.Map<DebitCard, DebitCardDto>((DebitCard)sales.FirstOrDefault(x => x.Id == s.Id).Payment);
                        break;
                    case Util.Enums.ePaymentTypes.Cheques:
                        s.Cheques = _mapper.Map<ChequesPayment, ChequesPaymentDto>((ChequesPayment)sales.FirstOrDefault(x => x.Id == s.Id).Payment);
                        s.Cheques.ListOfCheques =
                            _mapper.Map<IEnumerable<Cheque>, IEnumerable<ChequeDto>>(
                                (await _chequeRepository.GetAll()).Where(x => x.ChequesPaymentId == s.Cheques.Id)
                            ).ToList();
                        break;
                    default:
                        break;
                }
            }
            return salesDto;
        }
    }
}
