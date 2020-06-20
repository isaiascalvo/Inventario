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
    public class CategoryUseCases : IRaceUseCases
    {
        private readonly ICategoryRepository __categoryRepositoryRepository;
        private readonly IMapper _mapper;

        public CategoryUseCases(ICategoryRepository categoryRepository, IMapper mapper)
        {
            __categoryRepositoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<RaceDto> Create(RaceForCreationDto raceForCreationDto)
        {
            var race = new Data.Category()
            {
                Name = raceForCreationDto.Name,
                Description = raceForCreationDto.Description
            };

            return _mapper.Map<Data.Category, RaceDto>(await __categoryRepositoryRepository.Add(race));
        }

        public async Task Delete(Guid id)
        {
            var race = await __categoryRepositoryRepository.Delete(id);
            if (race == null)
                throw new KeyNotFoundException($"Race with id: {id} not found.");
        }

        public async Task<IEnumerable<RaceDto>> GetAll()
        {
            var races = await __categoryRepositoryRepository.GetAll();
            var racesDto = _mapper.Map<IEnumerable<Data.Category>, IEnumerable<RaceDto>>(races);
            return racesDto;
        }

        public async Task<RaceDto> GetOne(Guid id)
        {
            var race = await __categoryRepositoryRepository.GetById(id);
            if (race == null)
                throw new KeyNotFoundException($"Race with id: {id} not found.");

            return _mapper.Map<Data.Category, RaceDto>(race);
        }

        public async Task Update(Guid id, RaceDto raceDto)
        {
            var race = await __categoryRepositoryRepository.GetById(id);
            race.Name = raceDto.Name;
            race.Description = raceDto.Description;
            await __categoryRepositoryRepository.Update(race);
        }
    }
}
