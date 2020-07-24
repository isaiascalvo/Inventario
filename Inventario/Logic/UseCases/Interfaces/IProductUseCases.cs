using Data;
using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IProductUseCases
    {
        Task<ProductDto> GetOne(Guid id);
        Task<IEnumerable<ProductDto>> GetAll();
        Task<IEnumerable<ProductDto>> GetByCategory(Guid categoryId);
        Task Delete(Guid userId, Guid id);
        Task<ProductDto> Create(Guid userId, ProductForCreationDto productForCreationDto);
        Task Update(Guid id, ProductDto productDto);
    }
}
