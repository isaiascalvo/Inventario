using Data;
using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IAnimalUseCases
    {
        Task<AnimalDto> GetOne(Guid id);
        Task<IEnumerable<AnimalDto>> GetAll();
        Task<IEnumerable<AnimalDto>> GetByRace(Guid raceId);
        Task Delete(Guid id);
        Task<AnimalDto> Create(AnimalForCreationDto animalDto);
        Task Update(Guid id, AnimalDto animalDto);
    }
}
