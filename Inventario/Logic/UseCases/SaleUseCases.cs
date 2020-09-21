using AutoMapper;
using Data;
using Data.Models;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Enums;

namespace Logic
{
    public class SaleUseCases : ISaleUseCases
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IDetailRepository _detailRepository;
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
            IDetailRepository detailRepository,
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
            _detailRepository = detailRepository;
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
                Client client = null;
                if (saletForCreationDto.ClientId.HasValue)
                {
                    client = await _clientRepository.GetById(saletForCreationDto.ClientId.Value);
                    if (client == null)
                        throw new KeyNotFoundException($"Client with id: {saletForCreationDto.ClientId} not found.");
                }

                var sale = new Sale()
                {
                    ClientId = saletForCreationDto.ClientId,
                    ClientName = client != null ? client.Name + " " + client.Lastname : saletForCreationDto.ClientName,
                    Date = saletForCreationDto.Date.ToLocalTime(),
                    PaymentType = saletForCreationDto.PaymentType,
                    CreatedBy = userId
                };

                double total = 0;
                foreach (var detailFC in saletForCreationDto.Details)
                {
                    var product = await _productRepository.GetById(detailFC.ProductId);
                    if (product == null)
                        throw new KeyNotFoundException($"Product with id: {detailFC.ProductId} not found.");

                    var price = (await _priceRepository.GetAll())
                        .OrderByDescending(x => x.DateTime)
                        .FirstOrDefault(
                            x => x.ProductId == detailFC.ProductId &&
                            x.DateTime.ToLocalTime() <= saletForCreationDto.Date.ToLocalTime() &&
                            !x.IsDeleted
                        );

                    var detail = new Detail()
                    {
                        SaleId = sale.Id,
                        ProductId = detailFC.ProductId,
                        Quantity = detailFC.Quantity,
                        CreatedBy = userId
                    };
                    product.Stock -= detailFC.Quantity;

                    if (sale.PaymentType == ePaymentTypes.OwnFees)
                    {
                        FeeRule feeRule = (await _feeRuleRepository.Find(x => x.ProductId == detailFC.ProductId))
                            .OrderByDescending(x => x.Date).ThenBy(x => x.FeesAmountTo)
                            .FirstOrDefault(x => x.Date <= sale.Date && x.FeesAmountTo >= saletForCreationDto.OwnFees.Quantity);
                        if (feeRule == null)
                            throw new KeyNotFoundException($"Fee Rule not found.");

                        double percentage = feeRule.Percentage * saletForCreationDto.OwnFees.Quantity / 100;
                        total += price.Value * detailFC.Quantity * (1 + percentage);
                    }
                    else
                        total += price.Value * detailFC.Quantity;

                    detail = await _detailRepository.Add(detail);
                    await _productRepository.Update(product);
                }

                Payment payment = null;
                switch (saletForCreationDto.PaymentType)
                {
                    case Util.Enums.ePaymentTypes.Cash:
                        var cashDto = (CashForCreationDto)saletForCreationDto.Cash;
                        payment = new Cash(total, cashDto.Discount)
                        {
                            SaleId = sale.Id,
                            CreatedBy = userId
                        };

                        await _cashRepository.Add((Cash)payment);
                        break;
                    case Util.Enums.ePaymentTypes.OwnFees:
                        var ownFeesDto = (OwnFeesForCreationDto)saletForCreationDto.OwnFees;
                        payment = new OwnFees(ownFeesDto.ExpirationDate, total, ownFeesDto.Quantity, userId)
                        {
                            SaleId = sale.Id,
                        };

                        payment = await _ownFeesRepository.Add((OwnFees)payment);
                        foreach (var fee in ((OwnFees)payment).FeeList)
                        {
                            await _feeRepository.Add(fee);
                        }
                        break;
                    case Util.Enums.ePaymentTypes.CreditCard:
                        var creditCardDto = (CreditCardForCreationDto)saletForCreationDto.CreditCard;
                        payment = new CreditCard(total, creditCardDto.Discount, creditCardDto.Surcharge)
                        {
                            SaleId = sale.Id,
                            CardType = creditCardDto.CardType,
                            Bank = creditCardDto.Bank,
                            CreatedBy = userId
                        };

                        await _creditCardRepository.Add((CreditCard)payment);
                        break;
                    case Util.Enums.ePaymentTypes.DebitCard:
                        var debitCardDto = (DebitCardForCreationDto)saletForCreationDto.DebitCard;
                        payment = new DebitCard(total, debitCardDto.Discount, debitCardDto.Surcharge)
                        {
                            SaleId = sale.Id,
                            CardType = debitCardDto.CardType,
                            Bank = debitCardDto.Bank,
                            CreatedBy = userId
                        };

                        await _debitCardRepository.Add((DebitCard)payment);
                        break;
                    case Util.Enums.ePaymentTypes.Cheques:
                        var chequesDto = (ChequesPaymentForCreationDto)saletForCreationDto.Cheques;

                        if (chequesDto.ListOfCheques.Sum(x => x.Value) != total)
                            throw new InvalidOperationException("The sum of cheques list is different of amount.");

                        payment = new ChequesPayment()
                        {
                            SaleId = sale.Id,
                            Amount = total,
                            CreatedBy = userId
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
                                CreatedBy = userId
                            };

                            await _chequeRepository.Add(cheque);
                        }
                        break;
                    default:
                        break;
                }

                sale.PaymentId = payment.Id;
                sale = await _saleRepository.Add(sale);

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
            var sale = await _saleRepository.GetById(id, x => x.Include(s => s.Details));
            if (sale == null)
                throw new KeyNotFoundException($"Sale with id: {id} not found.");

            foreach (var detail in sale.Details)
            {
                var product = await _productRepository.GetById(detail.ProductId);
                if (product == null)
                    product = (await _productRepository.FindDeleted(x => x.Id == detail.ProductId)).FirstOrDefault();
                if (product == null)
                    throw new KeyNotFoundException($"Product with id: {detail.ProductId} not found.");

                product.Stock += detail.Quantity;

                await _productRepository.Update(product);
                await _detailRepository.Delete(userId, detail.Id);
            }

            Payment payment = null;
            switch (sale.PaymentType)
            {
                case Util.Enums.ePaymentTypes.Cash:
                    payment = await _cashRepository.Delete(userId, sale.PaymentId);
                    break;
                case Util.Enums.ePaymentTypes.OwnFees:
                    payment = await _ownFeesRepository.Delete(userId, sale.PaymentId);
                    List<Fee> fees = await _feeRepository.Find(x => x.OwnFeesId == payment.Id);
                    foreach (var fee in fees)
                    {
                        await _feeRepository.Delete(userId, fee.Id);
                    }
                    break;
                case Util.Enums.ePaymentTypes.CreditCard:
                    payment = await _creditCardRepository.Delete(userId, sale.PaymentId);
                    break;
                case Util.Enums.ePaymentTypes.DebitCard:
                    payment = await _debitCardRepository.Delete(userId, sale.PaymentId);
                    break;
                case Util.Enums.ePaymentTypes.Cheques:
                    payment = await _chequesPaymentRepository.Delete(userId, sale.PaymentId);
                    List<Cheque> cheques = await _chequeRepository.Find(x => x.ChequesPaymentId == payment.Id);
                    foreach (var cheque in cheques)
                    {
                        await _chequeRepository.Delete(userId, cheque.Id);
                    }
                    break;
                default:
                    break;
            }

            if (payment == null)
                throw new KeyNotFoundException($"Payment with id: {payment.Id} not found.");

            await _saleRepository.Delete(userId, id);
            await _saleRepository.CommitAsync();
        }

        public async Task<IEnumerable<SaleDto>> GetAll()
        {
            try
            {
                var sales = (await _saleRepository.GetAll(x => x.Include(s => s.Client).Include(s => s.Payment).Include(s => s.Details).ThenInclude(d => d.Product)))
                    .OrderByDescending(x => x.Date);
                foreach (var sale in sales)
                {
                    sale.Details = sale.Details.Where(x => !x.IsDeleted).ToList();
                }
                var salesDto = _mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDto>>(sales);
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
            return (await _saleRepository.GetAll(x => x.Include(s => s.Details).ThenInclude(d => d.Product))).AsQueryable().Where(filtersDto.GetExpresion()).Count();
        }

        public async Task<IEnumerable<SaleDto>> GetByPageAndQty(int skip, int qty)
        {
            var sales = (await _saleRepository.GetAll(x => x.Include(s => s.Client).Include(s => s.Payment).Include(s => s.Details).ThenInclude(d => d.Product)))
                .OrderByDescending(x => x.Date).Skip(skip).Take(qty);
            foreach (var sale in sales)
            {
                sale.Details = sale.Details.Where(x => !x.IsDeleted).ToList();
            }
            var salesDto = _mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDto>>(sales);
            return await SetSalePayments(sales, salesDto);
        }

        public async Task<IEnumerable<SaleDto>> GetFilteredByPageAndQty(SaleFiltersDto filtersDto, int skip, int qty)
        {
            var sales = (await _saleRepository.GetAll(x => x.Include(s => s.Client).Include(s => s.Payment).Include(s => s.Details).ThenInclude(d => d.Product)))
                .AsQueryable().Where(filtersDto.GetExpresion()).ToList().OrderByDescending(x => x.Date).Skip(skip).Take(qty);
            foreach (var sale in sales)
            {
                sale.Details = sale.Details.Where(x => !x.IsDeleted).ToList();
            }
            var salesDto = _mapper.Map<IEnumerable<Sale>, IEnumerable<SaleDto>>(sales);
            return await SetSalePayments(sales, salesDto);
        }

        public async Task<SaleDto> GetOne(Guid id)
        {
            var sale = await _saleRepository.GetById(id, x => x.Include(s => s.Client).Include(s => s.Payment).Include(s => s.Details).ThenInclude(d => d.Product));
            if (sale == null)
                throw new KeyNotFoundException($"Sale with id: {id} not found.");

            sale.Details = sale.Details.Where(x => !x.IsDeleted).ToList();

            var saleDto = _mapper.Map<Sale, SaleDto>(sale);

            var payment = sale.Payment;
            switch (saleDto.PaymentType)
            {
                case Util.Enums.ePaymentTypes.Cash:
                    saleDto.Cash = _mapper.Map<Cash, CashDto>((Cash)payment);
                    break;
                case Util.Enums.ePaymentTypes.OwnFees:
                    OwnFees owF = (OwnFees)payment;
                    owF.FeeList = (await _feeRepository.GetAll()).Where(x => x.OwnFeesId == owF.Id).OrderBy(x => x.ExpirationDate).ToList();
                    saleDto.OwnFees = _mapper.Map<OwnFees, OwnFeesDto>(owF);
                    break;
                case Util.Enums.ePaymentTypes.CreditCard:
                    saleDto.CreditCard = _mapper.Map<CreditCard, CreditCardDto>((CreditCard)payment);
                    break;
                case Util.Enums.ePaymentTypes.DebitCard:
                    saleDto.DebitCard = _mapper.Map<DebitCard, DebitCardDto>((DebitCard)payment);
                    break;
                case Util.Enums.ePaymentTypes.Cheques:
                    saleDto.Cheques = _mapper.Map<ChequesPayment, ChequesPaymentDto>((ChequesPayment)payment);
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
                var payment = sales.FirstOrDefault(x => x.Id == s.Id).Payment;
                switch (s.PaymentType)
                {
                    case Util.Enums.ePaymentTypes.Cash:
                        s.Cash = _mapper.Map<Cash, CashDto>((Cash)payment);
                        break;
                    case Util.Enums.ePaymentTypes.OwnFees:
                        OwnFees owF = (OwnFees)payment;
                        owF.FeeList = (await _feeRepository.GetAll()).Where(x => x.OwnFeesId == owF.Id).OrderBy(x => x.ExpirationDate).ToList();
                        s.OwnFees = _mapper.Map<OwnFees, OwnFeesDto>(owF);
                        break;
                    case Util.Enums.ePaymentTypes.CreditCard:
                        s.CreditCard = _mapper.Map<CreditCard, CreditCardDto>((CreditCard)payment);
                        break;
                    case Util.Enums.ePaymentTypes.DebitCard:
                        s.DebitCard = _mapper.Map<DebitCard, DebitCardDto>((DebitCard)payment);
                        break;
                    case Util.Enums.ePaymentTypes.Cheques:
                        s.Cheques = _mapper.Map<ChequesPayment, ChequesPaymentDto>((ChequesPayment)payment);
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

        public async Task PayFee(Guid userId, Guid feeId, DateTime paymentDate)
        {
            var fee = await _feeRepository.GetById(feeId);
            if (fee == null)
                throw new KeyNotFoundException($"Fee with id: {feeId} not found.");
            if (fee.PaymentDate != null)
                throw new InvalidOperationException($"Fee with id: {feeId} has already been paid.");

            var feeList = (await _feeRepository.GetAll()).Where(x => x.OwnFeesId == fee.OwnFeesId).OrderBy(x => x.ExpirationDate);

            var previousFee = feeList.FirstOrDefault(x => x.ExpirationDate < fee.ExpirationDate);
            if (previousFee != null && previousFee.PaymentDate == null)
                throw new InvalidOperationException($"The previous fee must be paid.");

            fee.PaymentDate = paymentDate;
            await _feeRepository.Update(fee);
            await _feeRepository.CommitAsync();
        }
    }
}
