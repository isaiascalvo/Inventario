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
            CreateMap<AnimalViewModel, ProductDto>();
            CreateMap<ProductDto, AnimalViewModel>()
                .ForMember(aVM => aVM.MotherName, aDto => aDto.MapFrom(src => src.Mother.Name))
                .ForMember(aVM => aVM.FatherName, aDto => aDto.MapFrom(src => src.Father.Name));
            CreateMap <AnimalForCreationVM, ProductForCreationDto>();
            //.ForMember(
            //    dest => dest.Name,
            //    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
            //);

            CreateMap<RaceViewModel, CategoryDto>();
            CreateMap<CategoryDto, RaceViewModel>();
            CreateMap<RaceForCreationViewModel, CategoryForCreationDto>();

            CreateMap<DiseaseViewModel, ClientDto>();
            CreateMap<ClientDto, DiseaseViewModel>();
            CreateMap<DiseaseForCreationViewModel, ClientForCreationDto>();

            CreateMap<PregnancyViewModel, VendorDto>();
            CreateMap<VendorDto, PregnancyViewModel>();
            CreateMap<PregnancyForCreationViewModel, VendorForCreationDto>();

            CreateMap<AnimalDiseaseViewModel, PriceDto>();
            CreateMap<PriceDto, AnimalDiseaseViewModel>();
            CreateMap<AnimalDiseaseForCreationViewModel, PriceForCreationDto>();
        }
    }
}
