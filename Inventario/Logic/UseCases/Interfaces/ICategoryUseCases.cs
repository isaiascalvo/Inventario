using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface ICategoryUseCases
    {
        Task<CategoryDto> GetOne(Guid id);
        Task<IEnumerable<CategoryDto>> GetAll();
        Task Delete(Guid userId, Guid id);
        Task<CategoryDto> Create(Guid userId, CategoryForCreationDto categoryForCreationDto);
        Task Update(Guid id, CategoryDto categoryDto);
        Task<int> GetTotalQty();
        Task<IEnumerable<CategoryDto>> GetByPageAndQty(int skip, int qty);
    }
}
