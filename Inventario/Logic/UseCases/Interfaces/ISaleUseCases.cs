﻿using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface ISaleUseCases
    {
        Task<SaleDto> GetOne(Guid id);
        Task<IEnumerable<SaleDto>> GetAll();
        //Task<IEnumerable<SaleDto>> GetByFilters(ClientFiltersDto filters);
        Task Delete(Guid userId, Guid id);
        Task<SaleDto> PreCreation(Guid userId, SaleForCreationDto saleForCreationDto);
        Task<SaleDto> Create(Guid userId, SaleForCreationDto saleForCreationDto);
        //Task Update(Guid id, SaleDto saleDto);
        Task<int> GetTotalQty();
        Task<IEnumerable<SaleDto>> GetByPageAndQty(int skip, int qty);
        Task<int> GetTotalQtyByFilters(SaleFiltersDto filtersDto);
        Task<IEnumerable<SaleDto>> GetFilteredByPageAndQty(SaleFiltersDto filtersDto, int skip, int qty);
        Task PayFee(Guid userId, Guid feeId, DateTime paymentDate);
    }
}
