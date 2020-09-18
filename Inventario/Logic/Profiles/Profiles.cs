using AutoMapper;
using Data;
using Data.Models;
using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
            //.ForMember(
            //    dest => dest.Name,
            //    opt => opt.MapFrom(src => $"{src.FirstName} {src.Lastname}")
            //);

            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();

            CreateMap<ClientDto, Client>();
            CreateMap<Client, ClientDto>();

            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<VendorDto, Vendor>();
            CreateMap<Vendor, VendorDto>();

            CreateMap<PriceDto, Price>();
            CreateMap<Price, PriceDto>();

            CreateMap<ProductEntryDto, ProductEntry>();
            CreateMap<ProductEntry, ProductEntryDto>();

            CreateMap<ProductEntryLineDto, ProductEntryLine>();
            CreateMap<ProductEntryLine, ProductEntryLineDto>();
            
            CreateMap<SaleDto, Sale>();
            CreateMap<Sale, SaleDto>();
            
            CreateMap<DetailDto, Detail>();
            CreateMap<Detail, DetailDto>();

            CreateMap<PaymentDto, Payment>();
            CreateMap<Payment, PaymentDto>();

            CreateMap<CashDto, Cash>();
            CreateMap<Cash, CashDto>();

            CreateMap<DebitCardDto, DebitCard>();
            CreateMap<DebitCard, DebitCardDto>();

            CreateMap<CreditCardDto, CreditCard>();
            CreateMap<CreditCard, CreditCardDto>();

            CreateMap<ChequesPaymentDto, ChequesPayment>();
            CreateMap<ChequesPayment, ChequesPaymentDto>();

            CreateMap<ChequeDto, Cheque>();
            CreateMap<Cheque, ChequeDto>();

            CreateMap<OwnFeesDto, OwnFees>();
            CreateMap<OwnFees, OwnFeesDto>();

            CreateMap<FeeDto, Fee>();
            CreateMap<Fee, FeeDto>();

            CreateMap<MiscellaneousExpensesDto, MiscellaneousExpenses>();
            CreateMap<MiscellaneousExpenses, MiscellaneousExpensesDto>();

            CreateMap<FeeRuleDto, FeeRule>();
            CreateMap<FeeRule, FeeRuleDto>();
            
            CreateMap<CommissionDto, Commission>();
            CreateMap<Commission, CommissionDto>();
        }
    }
}
