using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public interface IProductEntryUseCases
    {
        Task<ProductEntryDto> GetOne(Guid id);
        Task<IEnumerable<ProductEntryDto>> GetAll();
        Task Delete(Guid userId, Guid id);
        Task<ProductEntryDto> Create(Guid userId, ProductEntryForCreationDto productEntryForCreationDto);
        Task Update(Guid id, ProductEntryDto productEntryDto);
    }
}
