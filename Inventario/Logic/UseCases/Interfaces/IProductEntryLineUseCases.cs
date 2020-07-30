using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IProductEntryLineUseCases
    {
        Task<ProductEntryLineDto> GetOne(Guid id);
        Task<IEnumerable<ProductEntryLineDto>> GetAll();
        Task<IEnumerable<ProductEntryLineDto>> GetByProductEntry(Guid productEntryId);
        Task Delete(Guid userId, Guid id);
        Task<ProductEntryLineDto> Create(Guid userId, ProductEntryLineForCreationDto productEntryLineForCreationDto);
        Task Update(Guid id, ProductEntryLineDto productEntryLineDto);
    }
}
