using AutoMapper;
using Data;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class AnimalUseCases : IAnimalUseCases
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IAnimalDiseaseRepository _animalDiseaseRepository;
        private readonly IMapper _mapper;

        public AnimalUseCases(IAnimalRepository animalRepository, IAnimalDiseaseRepository animalDiseaseRepository, IMapper mapper)
        {
            _animalRepository = animalRepository;
            _animalDiseaseRepository = animalDiseaseRepository;
            _mapper = mapper;
        }

        public async Task<AnimalDto> Create(AnimalForCreationDto animalForCreationDto)
        {
            var animal = new Product()
            {
                Name = animalForCreationDto.Name,
                BirthDate = animalForCreationDto.BirthDate,
                FatherId = animalForCreationDto.FatherId,
                MotherId = animalForCreationDto.MotherId,
                RaceId = animalForCreationDto.RaceId,
                Number = animalForCreationDto.Number,
                Sex = animalForCreationDto.Sex,
                LeftProfileImg = animalForCreationDto.LeftProfileImg,
                FrontProfileImg = animalForCreationDto.FrontProfileImg,
                RightProfileImg = animalForCreationDto.RightProfileImg,
                BirthObservations = animalForCreationDto.BirthObservations,
                Alive = animalForCreationDto.Alive
            };

            return _mapper.Map<Product, AnimalDto>(await _animalRepository.Add(animal));
        }

        public async Task Delete(Guid id)
        {
            var animal = await _animalRepository.Delete(id);
            if (animal == null)
                throw new KeyNotFoundException($"Animal with id: {id} not found.");
        }

        public async Task<IEnumerable<AnimalDto>> GetAll()
        {
            var animals = (await _animalRepository.GetAll()).OrderBy(x => x.Name);
            var animalsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<AnimalDto>>(animals);
            return animalsDto;
        }

        public async Task<IEnumerable<AnimalDto>> GetByRace(Guid raceId)
        {
            var animals = (await _animalRepository.GetAll()).Where(x => x.RaceId == raceId).OrderBy(x => x.Name);
            var animalsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<AnimalDto>>(animals);
            return animalsDto;
        }

        public async Task<AnimalDto> GetOne(Guid id)
        {
            var animal = await _animalRepository.GetById(id, a => a.Race, a => a.Mother, a => a.Father);
            if (animal == null)
                throw new KeyNotFoundException($"Animal with id: {id} not found.");

            var animalDto = _mapper.Map<Product, AnimalDto>(animal);

            var animalDiseases = _animalDiseaseRepository.AllIncluding(ad => ad.Disease).Where(ad => ad.AnimalId == id && ad.EndDate == null).ToList();

            animalDto.AnimalDiseases = _mapper.Map<IEnumerable<Price>, IEnumerable<AnimalDiseaseDto>>(animalDiseases);

            return animalDto;
        }

        public async Task Update(Guid id, AnimalDto animalDto)
        {
            var animal = await _animalRepository.GetById(id);
            animal.Name = animalDto.Name;
            animal.BirthDate = animalDto.BirthDate;
            animal.FatherId = animalDto.FatherId;
            animal.MotherId = animalDto.MotherId;
            animal.RaceId = animalDto.RaceId;
            animal.Number = animalDto.Number;
            animal.Sex = animalDto.Sex;
            animal.LeftProfileImg = animalDto.LeftProfileImg;
            animal.FrontProfileImg = animalDto.FrontProfileImg;
            animal.RightProfileImg = animalDto.RightProfileImg;
            animal.BirthObservations = animalDto.BirthObservations;
            animal.Alive = animalDto.Alive;            
            await _animalRepository.Update(animal);
        }
    }
}
