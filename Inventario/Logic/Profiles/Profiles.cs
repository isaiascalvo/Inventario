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
            //    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
            //);

            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();

            CreateMap<ClientDto, Client>();
            CreateMap<Client, ClientDto>();

            CreateMap<VendorDto, Vendor>();
            CreateMap<Vendor, VendorDto>();

            CreateMap<PriceDto, Price>();
            CreateMap<Price, PriceDto>();
        }
    }
}
