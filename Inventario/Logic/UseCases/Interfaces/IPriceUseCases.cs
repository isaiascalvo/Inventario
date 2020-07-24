using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IPriceUseCases
    {
        Task<PriceDto> GetOne(Guid id);
        IEnumerable<PriceDto> GetAll();
        Task Delete(Guid userId, Guid id);
        Task<PriceDto> Create(Guid userId, PriceForCreationDto animalDiseaseDto);
        Task Update(Guid id, PriceDto animalDiseaseDto);
    }
}
