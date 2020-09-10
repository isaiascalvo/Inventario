using Data;
using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IProductUseCases
    {
        Task<ProductDto> GetOne(Guid id);
        Task<IEnumerable<ProductDto>> GetAll();
        Task<int> GetTotalQty();
        Task<IEnumerable<ProductDto>> GetByPageAndQty(int skip, int qty);
        Task<IEnumerable<ProductDto>> GetByCategory(Guid categoryId);
        Task Delete(Guid userId, Guid id);
        Task<ProductDto> Create(Guid userId, ProductForCreationDto productForCreationDto);
        Task Update(Guid id, ProductDto productDto);
        Task<IEnumerable<ProductDto>> GetByFilters(ProductFiltersDto filters);
        Task<int> GetTotalQtyByFilters(ProductFiltersDto filters);
        Task<IEnumerable<ProductDto>> GetFilteredByPageAndQty(ProductFiltersDto filters, int skip, int qty);
        Task<double> GetPriceByDate(Guid productId, DateTime date);
        Task<MemoryStream> CreatePdf(ProductFiltersDto filtersDto);
    }
}
