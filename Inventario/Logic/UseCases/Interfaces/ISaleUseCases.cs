using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface ISaleUseCases
    {
        Task<SaleDto> GetOne(Guid id);
        Task<IEnumerable<SaleDto>> GetAll();
        //Task<IEnumerable<SaleDto>> GetByFilters(ClientFiltersDto filters);
        Task Delete(Guid userId, Guid id);
        Task<SaleDto> Create(Guid userId, SaleForCreationDto saleDto);
        Task Update(Guid id, SaleDto saleDto);
    }
}
