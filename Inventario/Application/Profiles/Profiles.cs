using Application.ViewModels;
using AutoMapper;
using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<ProductViewModel, ProductDto>();
            //.ForMember(pDto => pDto.Price, pVM => pVM.MapFrom(src => src.Price))
            //.ForMember(pDto => pDto.Category, pVM => pVM.MapFrom(src => src.Category))
            //.ForMember(pDto => pDto.Vendor, pVM => pVM.MapFrom(src => src.Vendor));
            CreateMap<ProductDto, ProductViewModel>();
                //.ForMember(pVM => pVM.Price, pDto => pDto.MapFrom(src => src.Price))
                //.ForMember(pVM => pVM.Category, pDto => pDto.MapFrom(src => src.Category))
                //.ForMember(pVM => pVM.Vendor, pDto => pDto.MapFrom(src => src.Vendor));
            CreateMap<ProductForCreationViewModel, ProductForCreationDto>();
            //.ForMember(
            //    dest => dest.Name,
            //    opt => opt.MapFrom(src => $"{src.FirstName} {src.Lastname}")
            //);

            CreateMap<PriceViewModel, PriceDto>();
            CreateMap<PriceDto, PriceViewModel>();
            CreateMap<PriceForCreationViewModel, PriceForCreationDto>();

            CreateMap<CategoryViewModel, CategoryDto>();
            CreateMap<CategoryDto, CategoryViewModel>();
            CreateMap<CategoryForCreationViewModel, CategoryForCreationDto>();

            CreateMap<VendorViewModel, VendorDto>();
            CreateMap<VendorDto, VendorViewModel>();
            CreateMap<VendorForCreationViewModel, VendorForCreationDto>();

            CreateMap<ClientViewModel, ClientDto>();
            CreateMap<ClientDto, ClientViewModel>();
            CreateMap<ClientForCreationViewModel, ClientForCreationDto>();

            CreateMap<UserViewModel, UserDto>();
            CreateMap<UserDto, UserViewModel>();
            CreateMap<UserForCreationViewModel, UserForCreationDto>();

            CreateMap<PriceViewModel, PriceDto>();
            CreateMap<PriceDto, PriceViewModel>();
            CreateMap<PriceForCreationViewModel, PriceForCreationDto>();

            CreateMap<ProductEntryViewModel, ProductEntryDto>();
            CreateMap<ProductEntryDto, ProductEntryViewModel>();
            CreateMap<ProductEntryForCreationViewModel, ProductEntryForCreationDto>();

            CreateMap<ProductEntryLineViewModel, ProductEntryLineDto>();
            CreateMap<ProductEntryLineDto, ProductEntryLineViewModel>();
            CreateMap<ProductEntryLineForCreationViewModel, ProductEntryLineForCreationDto>();

            CreateMap<SaleViewModel, SaleDto>();
            CreateMap<SaleDto, SaleViewModel>();
            CreateMap<SaleForCreationViewModel, SaleForCreationDto>();
            
            CreateMap<DetailViewModel, DetailDto>();
            CreateMap<DetailDto, DetailViewModel>();
            CreateMap<DetailForCreationViewModel, DetailForCreationDto>();

            CreateMap<CashViewModel, CashDto>();
            CreateMap<CashDto, CashViewModel>();
            CreateMap<CashForCreationViewModel, CashForCreationDto>();

            CreateMap<DebitCardViewModel, DebitCardDto>();
            CreateMap<DebitCardDto, DebitCardViewModel>();
            CreateMap<DebitCardForCreationViewModel, DebitCardForCreationDto>();

            CreateMap<CreditCardViewModel, CreditCardDto>();
            CreateMap<CreditCardDto, CreditCardViewModel>();
            CreateMap<CreditCardForCreationViewModel, CreditCardForCreationDto>();

            CreateMap<ChequesPaymentViewModel, ChequesPaymentDto>();
            CreateMap<ChequesPaymentDto, ChequesPaymentViewModel>();
            CreateMap<ChequesPaymentForCreationViewModel, ChequesPaymentForCreationDto>();
            
            CreateMap<ChequeViewModel, ChequeDto>();
            CreateMap<ChequeDto, ChequeViewModel>();
            CreateMap<ChequeForCreationViewModel, ChequeForCreationDto>();

            CreateMap<OwnFeesViewModel, OwnFeesDto>();
            CreateMap<OwnFeesDto, OwnFeesViewModel>();
            CreateMap<OwnFeesForCreationViewModel, OwnFeesForCreationDto>();

            CreateMap<FeeViewModel, FeeDto>();
            CreateMap<FeeDto, FeeViewModel>();
            CreateMap<FeeForCreationViewModel, FeeForCreationDto>();
            
            CreateMap<MiscellaneousExpensesViewModel, MiscellaneousExpensesDto>();
            CreateMap<MiscellaneousExpensesDto, MiscellaneousExpensesViewModel>();
            CreateMap<MiscellaneousExpensesViewModel, MiscellaneousExpensesDto>();

            CreateMap<FeeRuleViewModel, FeeRuleDto>();
            CreateMap<FeeRuleDto, FeeRuleViewModel>();
            CreateMap<FeeRuleForCreationViewModel, FeeRuleForCreationDto>();

            CreateMap<FeeRuleByCategoryViewModel, FeeRuleByCategoryDto>();

            CreateMap<ProductFiltersViewModel, ProductFiltersDto>()
                .ForMember(
                    dest => dest.Date,
                    opt => opt.MapFrom(src => DateTime.Parse(src.Date))
                );

            CreateMap<SaleFiltersViewModel, SaleFiltersDto>()
                .ForMember(
                    dest => dest.DateFrom,
                    opt => opt.MapFrom(src => DateTime.Parse(src.DateFrom))
                )
                .ForMember(
                    dest => dest.DateTo,
                    opt => opt.MapFrom(src => DateTime.Parse(src.DateTo))
                );
            
            CreateMap<ProductEntryFiltersViewModel, ProductEntryFiltersDto>()
                .ForMember(
                    dest => dest.DateFrom,
                    opt => opt.MapFrom(src => DateTime.Parse(src.DateFrom))
                )
                .ForMember(
                    dest => dest.DateTo,
                    opt => opt.MapFrom(src => DateTime.Parse(src.DateTo))
                );
            
            CreateMap<MiscellaneousExpensesFiltersViewModel, MiscellaneousExpensesFiltersDto>()
                .ForMember(
                    dest => dest.DateFrom,
                    opt => opt.MapFrom(src => DateTime.Parse(src.DateFrom))
                )
                .ForMember(
                    dest => dest.DateTo,
                    opt => opt.MapFrom(src => DateTime.Parse(src.DateTo))
                );
            
            CreateMap<CommissionFiltersViewModel, CommissionFiltersDto>()
                .ForMember(
                    dest => dest.DateFrom,
                    opt => opt.MapFrom(src => DateTime.Parse(src.DateFrom))
                )
                .ForMember(
                    dest => dest.DateTo,
                    opt => opt.MapFrom(src => DateTime.Parse(src.DateTo))
                );

            CreateMap<CommissionViewModel, CommissionDto>();
            CreateMap<CommissionDto, CommissionViewModel>();
            CreateMap<CommissionForCreationViewModel, CommissionForCreationDto>();
            
            CreateMap<PeriodicReportViewModel, PeriodicReportDto>();
            CreateMap<PeriodicReportDto, PeriodicReportViewModel>();

            CreateMap<ClientFiltersViewModel, ClientFiltersDto>();
            CreateMap<VendorFiltersViewModel, VendorFiltersDto>();
            CreateMap<UserFiltersViewModel, UserFiltersDto>();
            CreateMap<FeeRuleFiltersViewModel, FeeRuleFiltersDto>();
        }
    }
}
