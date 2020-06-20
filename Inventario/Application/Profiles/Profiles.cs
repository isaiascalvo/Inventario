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
            CreateMap<AnimalViewModel, AnimalDto>();
            CreateMap<AnimalDto, AnimalViewModel>()
                .ForMember(aVM => aVM.MotherName, aDto => aDto.MapFrom(src => src.Mother.Name))
                .ForMember(aVM => aVM.FatherName, aDto => aDto.MapFrom(src => src.Father.Name));
            CreateMap <AnimalForCreationVM, AnimalForCreationDto>();
            //.ForMember(
            //    dest => dest.Name,
            //    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
            //);

            CreateMap<RaceViewModel, RaceDto>();
            CreateMap<RaceDto, RaceViewModel>();
            CreateMap<RaceForCreationViewModel, RaceForCreationDto>();

            CreateMap<DiseaseViewModel, DiseaseDto>();
            CreateMap<DiseaseDto, DiseaseViewModel>();
            CreateMap<DiseaseForCreationViewModel, DiseaseForCreationDto>();

            CreateMap<PregnancyViewModel, PregnancyDto>();
            CreateMap<PregnancyDto, PregnancyViewModel>();
            CreateMap<PregnancyForCreationViewModel, PregnancyForCreationDto>();

            CreateMap<AnimalDiseaseViewModel, AnimalDiseaseDto>();
            CreateMap<AnimalDiseaseDto, AnimalDiseaseViewModel>();
            CreateMap<AnimalDiseaseForCreationViewModel, AnimalDiseaseForCreationDto>();
        }
    }
}
