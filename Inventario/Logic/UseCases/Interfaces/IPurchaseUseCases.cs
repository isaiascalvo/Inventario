using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IPurchaseUseCases
    {
        Task<PurchaseDto> GetOne(Guid id);
        Task<IEnumerable<PurchaseDto>> GetAll();
        //Task<IEnumerable<PurchaseDto>> GetByFilters(ClientFiltersDto filters);
        Task Delete(Guid userId, Guid id);
        Task<PurchaseDto> Create(Guid userId, PurchaseForCreationDto purchaseDto);
        Task Update(Guid id, PurchaseDto purchaseDto);
    }
}
