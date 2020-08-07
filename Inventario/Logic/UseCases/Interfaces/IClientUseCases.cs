using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IClientUseCases
    {
        Task<ClientDto> GetOne(Guid id);
        Task<IEnumerable<ClientDto>> GetAll();
        Task<IEnumerable<ClientDto>> GetByFilters(ClientFiltersDto filters);
        Task Delete(Guid userId, Guid id);
        Task<ClientDto> Create(Guid userId, ClientForCreationDto diseaseDto);
        Task Update(Guid id, ClientDto diseaseDto);
    }
}
