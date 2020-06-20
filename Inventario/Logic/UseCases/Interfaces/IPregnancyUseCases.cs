using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IPregnancyUseCases
    {
        Task<PregnancyDto> GetOne(Guid id);
        Task<IEnumerable<PregnancyDto>> GetAll();
        Task Delete(Guid id);
        Task<PregnancyDto> Create(PregnancyForCreationDto pregnancyDto);
        Task Update(Guid id, PregnancyDto pregnancyDto);
    }
}
