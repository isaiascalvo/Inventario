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
            CreateMap<ProductDto, ProductViewModel>();
                //.ForMember(aVM => aVM.MotherName, aDto => aDto.MapFrom(src => src.Mother.Name))
                //.ForMember(aVM => aVM.FatherName, aDto => aDto.MapFrom(src => src.Father.Name));
            CreateMap <ProductForCreationViewModel, ProductForCreationDto>();
            //.ForMember(
            //    dest => dest.Name,
            //    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
            //);

            CreateMap<CategoryViewModel, CategoryDto>();
            CreateMap<CategoryDto, CategoryViewModel>();
            CreateMap<CategoryForCreationViewModel, CategoryForCreationDto>();

            CreateMap<VendorViewModel, VendorDto>();
            CreateMap<VendorDto, VendorViewModel>();
            CreateMap<VendorForCreationViewModel, VendorForCreationDto>();

            CreateMap<ClientViewModel, ClientDto>();
            CreateMap<ClientDto, ClientViewModel>();
            CreateMap<ClientForCreationViewModel, ClientForCreationDto>();

            CreateMap<PriceViewModel, PriceDto>();
            CreateMap<PriceDto, PriceViewModel>();
            CreateMap<PriceForCreationViewModel, PriceForCreationDto>();
        }
    }
}
