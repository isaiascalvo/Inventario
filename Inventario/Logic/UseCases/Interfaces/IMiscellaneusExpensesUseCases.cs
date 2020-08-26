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
    }
}
