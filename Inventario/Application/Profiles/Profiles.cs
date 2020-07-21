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
        }
    }
}
