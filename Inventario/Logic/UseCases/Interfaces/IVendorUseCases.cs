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
        Task Delete(Guid id);
        Task<VendorDto> Create(VendorForCreationDto pregnancyDto);
        Task Update(Guid id, VendorDto pregnancyDto);
    }
}
