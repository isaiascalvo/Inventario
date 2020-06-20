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
            CreateMap<AnimalDto, Product>();
            CreateMap<Product, AnimalDto>();
            //.ForMember(
            //    dest => dest.Name,
            //    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
            //);

            CreateMap<RaceDto, Data.Category>();
            CreateMap<Data.Category, RaceDto>();

            CreateMap<DiseaseDto, Client>();
            CreateMap<Client, DiseaseDto>();

            CreateMap<PregnancyDto, Vendor>();
            CreateMap<Vendor, PregnancyDto>();

            CreateMap<AnimalDiseaseDto, Price>();
            CreateMap<Price, AnimalDiseaseDto>();
        }
    }
}
