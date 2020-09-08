using Data.Models;
using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IMiscellaneousExpensesUseCases
    {
        Task<MiscellaneousExpensesDto> GetOne(Guid id);
        Task<IEnumerable<MiscellaneousExpensesDto>> GetAll();
        Task Delete(Guid userId, Guid id);
        Task<MiscellaneousExpensesDto> Create(Guid userId, MiscellaneousExpensesForCreationDto miscellaneousExpensesForCreationDto);
        Task Update(Guid id, MiscellaneousExpensesDto miscellaneousExpensesDto);
        Task<int> GetTotalQty();
        Task<IEnumerable<MiscellaneousExpensesDto>> GetByPageAndQty(int skip, int qty);
        Task<int> GetTotalQtyByFilters(MiscellaneousExpensesFiltersDto filtersDto);
        Task<IEnumerable<MiscellaneousExpensesDto>> GetFilteredByPageAndQty(MiscellaneousExpensesFiltersDto filtersDto, int skip, int qty);
    }
}
