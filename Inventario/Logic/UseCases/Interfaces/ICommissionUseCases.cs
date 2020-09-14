using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface ICommissionUseCases
    {
        Task<CommissionDto> GetOne(Guid id);
        Task<IEnumerable<CommissionDto>> GetAll();
        Task Delete(Guid userId, Guid id);
        Task<CommissionDto> Create(Guid userId, CommissionForCreationDto commissionForCreationDto);
        Task Update(Guid id, CommissionDto commissionDto);
        Task<int> GetTotalQty();
        Task<IEnumerable<CommissionDto>> GetByPageAndQty(int skip, int qty);
        Task<int> GetTotalQtyByFilters(CommissionFiltersDto filtersDto);
        Task<IEnumerable<CommissionDto>> GetFilteredByPageAndQty(CommissionFiltersDto filtersDto, int skip, int qty);
    }
}
