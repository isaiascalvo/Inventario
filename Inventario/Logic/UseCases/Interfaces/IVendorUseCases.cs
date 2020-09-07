using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IVendorUseCases
    {
        Task<VendorDto> GetOne(Guid id);
        Task<IEnumerable<VendorDto>> GetAll();
        Task Delete(Guid userId, Guid id);
        Task<VendorDto> Create(Guid userId, VendorForCreationDto vendorDto);
        Task Update(Guid id, VendorDto vendorDto);
        Task<int> GetTotalQty();
        Task<int> GetTotalQtyByFilters(VendorFiltersDto filtersDto);
        Task<IEnumerable<VendorDto>> GetByPageAndQty(int skip, int qty);
        Task<IEnumerable<VendorDto>> GetFilteredByPageAndQty(VendorFiltersDto filtersDto, int skip, int qty);
    }
}
