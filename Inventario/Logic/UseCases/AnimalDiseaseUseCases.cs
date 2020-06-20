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
    public class AnimalDiseaseUseCases: IAnimalDiseaseUseCases
    {
        private readonly IAnimalDiseaseRepository _animalDiseaseRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IMapper _mapper;

        public AnimalDiseaseUseCases(IAnimalDiseaseRepository animalDiseaseRepository, IAnimalRepository animalRepository, IDiseaseRepository diseaseRepository, IMapper mapper)
        {
            _animalDiseaseRepository = animalDiseaseRepository;
            _animalRepository = animalRepository;
            _diseaseRepository = diseaseRepository;
            _mapper = mapper;
        }

        public async Task<AnimalDiseaseDto> Create(AnimalDiseaseForCreationDto animalDiseaseForCreationDto)
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

            return _mapper.Map<Price, AnimalDiseaseDto>(await _animalDiseaseRepository.Add(animalDisease));
        }

        public async Task Delete(Guid id)
        {
            var animalDisease = await _animalDiseaseRepository.Delete(id);
            if (animalDisease == null)
                throw new KeyNotFoundException($"AnimalDisease with id: {id} not found.");
        }

        public async Task<IEnumerable<AnimalDiseaseDto>> GetAll()
        {
            var animalDisease = _animalDiseaseRepository.AllIncluding(ad => ad.Animal, ad => ad.Disease);
            var animalDiseaseDto = _mapper.Map<IEnumerable<Price>, IEnumerable<AnimalDiseaseDto>>(animalDisease);
            return animalDiseaseDto;
        }

        public async Task<AnimalDiseaseDto> GetOne(Guid id)
        {
            var animalDisease = await _animalDiseaseRepository.GetById(id);
            if (animalDisease == null)
                throw new KeyNotFoundException($"AnimalDiseasee with id: {id} not found.");

            return _mapper.Map<Price, AnimalDiseaseDto>(animalDisease);
        }

        public async Task Update(Guid id, AnimalDiseaseDto animalDiseaseDto)
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
