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
        Task Delete(Guid id);
        Task<CategoryDto> Create(CategoryForCreationDto raceDto);
        Task Update(Guid id, CategoryDto raceDto);
    }
}
