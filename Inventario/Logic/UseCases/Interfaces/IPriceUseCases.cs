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
        Task<IEnumerable<PriceDto>> GetAll();
        Task Delete(Guid id);
        Task<PriceDto> Create(PriceForCreationDto animalDiseaseDto);
        Task Update(Guid id, PriceDto animalDiseaseDto);
    }
}
