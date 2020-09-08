using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IFeeRuleUseCases
    {
        Task<FeeRuleDto> GetOne(Guid id);
        Task<IEnumerable<FeeRuleDto>> GetAll();
        Task<IEnumerable<FeeRuleDto>> GetByProduct(Guid productId);
        Task Delete(Guid userId, Guid id);
        Task<FeeRuleDto> Create(Guid userId, FeeRuleForCreationDto feeRuleForCreationDto);
        Task Update(Guid id, FeeRuleDto feeRuleDto);
        Task CreateByCategory(Guid userId, FeeRuleByCategoryDto feeRuleByCategoryDto);
        Task<IEnumerable<FeeRuleDto>> GetByPageAndQty(int skip, int qty);
        Task<int> GetTotalQty();
        Task<int> GetTotalQtyByFilters(FeeRuleFiltersDto filtersDto);
        Task<IEnumerable<FeeRuleDto>> GetFilteredByPageAndQty(FeeRuleFiltersDto filtersDto, int skip, int qty);
    }
}
