using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IDiseaseUseCases
    {
        Task<DiseaseDto> GetOne(Guid id);
        Task<IEnumerable<DiseaseDto>> GetAll();
        Task Delete(Guid id);
        Task<DiseaseDto> Create(DiseaseForCreationDto diseaseDto);
        Task Update(Guid id, DiseaseDto diseaseDto);
    }
}
