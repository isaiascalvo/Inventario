using AutoMapper;
using Data;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class PriceUseCases: IPriceUseCases
    {
        private readonly IPriceRepository _animalDiseaseRepository;
        private readonly IProductRepository _animalRepository;
        private readonly IClientRepository _diseaseRepository;
        private readonly IMapper _mapper;

        public PriceUseCases(IPriceRepository animalDiseaseRepository, IProductRepository animalRepository, IClientRepository diseaseRepository, IMapper mapper)
        {
            _animalDiseaseRepository = animalDiseaseRepository;
            _animalRepository = animalRepository;
            _diseaseRepository = diseaseRepository;
            _mapper = mapper;
        }

        public async Task<PriceDto> Create(PriceForCreationDto animalDiseaseForCreationDto)
        {
            var animal = await _animalRepository.GetById(animalDiseaseForCreationDto.AnimalId);
            if (animal == null)
                throw new KeyNotFoundException($"Animal with id: {animalDiseaseForCreationDto.AnimalId} not found.");
            var disease = await _diseaseRepository.GetById(animalDiseaseForCreationDto.DiseaseId);
            if(disease == null)
                throw new KeyNotFoundException($"Disease with id: {animalDiseaseForCreationDto.DiseaseId} not found.");

            var animalDisease = new Price()
            {
                AnimalId = animalDiseaseForCreationDto.AnimalId,
                DiseaseId = animalDiseaseForCreationDto.DiseaseId,
                StartDate = animalDiseaseForCreationDto.StartDate,
                EndDate = animalDiseaseForCreationDto.EndDate,
                Treatement = animalDiseaseForCreationDto.Treatement,
                Observations = animalDiseaseForCreationDto.Observations
            };

            return _mapper.Map<Price, PriceDto>(await _animalDiseaseRepository.Add(animalDisease));
        }

        public async Task Delete(Guid id)
        {
            var animalDisease = await _animalDiseaseRepository.Delete(id);
            if (animalDisease == null)
                throw new KeyNotFoundException($"AnimalDisease with id: {id} not found.");
        }

        public async Task<IEnumerable<PriceDto>> GetAll()
        {
            var animalDisease = _animalDiseaseRepository.AllIncluding(ad => ad.Animal, ad => ad.Disease);
            var animalDiseaseDto = _mapper.Map<IEnumerable<Price>, IEnumerable<PriceDto>>(animalDisease);
            return animalDiseaseDto;
        }

        public async Task<PriceDto> GetOne(Guid id)
        {
            var animalDisease = await _animalDiseaseRepository.GetById(id);
            if (animalDisease == null)
                throw new KeyNotFoundException($"AnimalDiseasee with id: {id} not found.");

            return _mapper.Map<Price, PriceDto>(animalDisease);
        }

        public async Task Update(Guid id, PriceDto animalDiseaseDto)
        {
            var animalDisease = await _animalDiseaseRepository.GetById(id);
            animalDisease.AnimalId = animalDiseaseDto.AnimalId;
            animalDisease.DiseaseId = animalDiseaseDto.DiseaseId;
            animalDisease.StartDate = animalDiseaseDto.StartDate;
            animalDisease.EndDate = animalDiseaseDto.EndDate;
            animalDisease.Treatement = animalDiseaseDto.Treatement;
            animalDisease.Observations = animalDiseaseDto.Observations;
            await _animalDiseaseRepository.Update(animalDisease);
        }
    }
}
