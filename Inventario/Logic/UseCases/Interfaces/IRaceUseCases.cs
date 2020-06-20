using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IRaceUseCases
    {
        Task<RaceDto> GetOne(Guid id);
        Task<IEnumerable<RaceDto>> GetAll();
        Task Delete(Guid id);
        Task<RaceDto> Create(RaceForCreationDto raceDto);
        Task Update(Guid id, RaceDto raceDto);
    }
}
