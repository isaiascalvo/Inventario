using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IAnimalDiseaseUseCases
    {
        Task<AnimalDiseaseDto> GetOne(Guid id);
        Task<IEnumerable<AnimalDiseaseDto>> GetAll();
        Task Delete(Guid id);
        Task<AnimalDiseaseDto> Create(AnimalDiseaseForCreationDto animalDiseaseDto);
        Task Update(Guid id, AnimalDiseaseDto animalDiseaseDto);
    }
}
