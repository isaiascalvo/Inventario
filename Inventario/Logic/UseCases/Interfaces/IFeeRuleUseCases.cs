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
        Task Delete(Guid userId, Guid id);
        Task<FeeRuleDto> Create(Guid userId, FeeRuleForCreationDto feeRuleForCreationDto);
        Task Update(Guid id, FeeRuleDto feeRuleDto);
        Task CreateByCategory(Guid userId, FeeRuleByCategoryDto feeRuleByCategoryDto);
    }
}
