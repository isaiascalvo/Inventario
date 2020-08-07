using AutoMapper;
using Data;
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
            
            CreateMap<PurchaseDto, Purchase>();
            CreateMap<Purchase, PurchaseDto>();
        }
    }
}
