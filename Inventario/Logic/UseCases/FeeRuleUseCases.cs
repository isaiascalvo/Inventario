using AutoMapper;
using Data;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class FeeRuleUseCases : IFeeRuleUseCases
    {
        private readonly IFeeRuleRepository _feeRuleRepositoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public FeeRuleUseCases(IFeeRuleRepository feeRuleRepositoryRepository, IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _feeRuleRepositoryRepository = feeRuleRepositoryRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<FeeRuleDto> Create(Guid userId, FeeRuleForCreationDto feeRuleForCreationDto)
        {
            var product = await _productRepository.GetById(feeRuleForCreationDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {feeRuleForCreationDto.ProductId} not found.");

            var feeRule = new FeeRule()
            {
                ProductId = feeRuleForCreationDto.ProductId,
                Product = product,
                Date = DateTime.Now.ToLocalTime(),
                FeesAmountTo = feeRuleForCreationDto.FeesAmountTo,
                Percentage = feeRuleForCreationDto.Percentage,
                CreatedBy = userId
            };

            feeRule = await _feeRuleRepositoryRepository.Add(feeRule);
            await _feeRuleRepositoryRepository.CommitAsync();
            return _mapper.Map<FeeRule, FeeRuleDto>(feeRule);
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var feeRule = await _feeRuleRepositoryRepository.Delete(userId, id);
            if (feeRule == null)
                throw new KeyNotFoundException($"Fee Rule with id: {id} not found.");

            await _feeRuleRepositoryRepository.CommitAsync();
        }

        public async Task<IEnumerable<FeeRuleDto>> GetAll()
        {
            
            var feeRules = await GetActiveFeeRules();

            feeRules = feeRules.OrderBy(x => x.Product.Name).ThenBy(x => x.FeesAmountTo).ToList();
            return _mapper.Map<List<FeeRule>, List<FeeRuleDto>>(feeRules);
        }

        public async Task<int> GetTotalQty()
        {
            return (await GetActiveFeeRules()).Count();
        }

        public async Task<int> GetTotalQtyByFilters(FeeRuleFiltersDto filtersDto)
        {
            return (await GetActiveFeeRules()).AsQueryable().Where(filtersDto.GetExpresion()).Count();
        }

        public async Task<IEnumerable<FeeRuleDto>> GetByPageAndQty(int skip, int qty)
        {
            var users = (await GetActiveFeeRules())
                .OrderByDescending(x => x.Product.Name).ThenBy(x => x.FeesAmountTo).Skip(skip).Take(qty);
            return _mapper.Map<IEnumerable<FeeRule>, IEnumerable<FeeRuleDto>>(users);
        }

        public async Task<IEnumerable<FeeRuleDto>> GetFilteredByPageAndQty(FeeRuleFiltersDto filtersDto, int skip, int qty)
        {
            var users = (await GetActiveFeeRules())
                .AsQueryable().Where(filtersDto.GetExpresion()).ToList()
                .OrderByDescending(x => x.Product.Name).ThenBy(x => x.FeesAmountTo).Skip(skip).Take(qty);
            return _mapper.Map<IEnumerable<FeeRule>, IEnumerable<FeeRuleDto>>(users);
        }

        public async Task<IEnumerable<FeeRuleDto>> GetByProducts(List<Guid> productsIds)
        {
            List<FeeRule> feeRules = new List<FeeRule>();
            foreach (var productId in productsIds)
            {
                List<FeeRule> productFeeRules = await _feeRuleRepositoryRepository.Find(x => x.ProductId == productId);
                var groups = productFeeRules.GroupBy(x => x.FeesAmountTo);
                foreach (var group in groups)
                {
                    var fee = group.OrderByDescending(x => x.Date).FirstOrDefault();
                    if (fee != null)
                    {
                        feeRules.Add(fee);
                    }
                }
            }
            
            feeRules = feeRules.OrderBy(x => x.ProductId).ThenBy(x => x.FeesAmountTo).ToList();
            return _mapper.Map<List<FeeRule>, List<FeeRuleDto>>(feeRules);
        }

        public async Task<FeeRuleDto> GetOne(Guid id)
        {
            var feeRule = await _feeRuleRepositoryRepository.GetById(id);
            if (feeRule == null)
                throw new KeyNotFoundException($"Fee Rule with id: {id} not found."); 

            return _mapper.Map<FeeRule, FeeRuleDto>(feeRule);
        }

        public async Task Update(Guid id, FeeRuleDto feeRuleDto)
        {
            var product = await _productRepository.GetById(feeRuleDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with id: {feeRuleDto.ProductId} not found.");

            var feeRule = new FeeRule()
            {
                ProductId = feeRuleDto.ProductId,
                Product = product,
                Date = DateTime.Now.ToLocalTime(),
                FeesAmountTo = feeRuleDto.FeesAmountTo,
                Percentage = feeRuleDto.Percentage,
                CreatedBy = feeRuleDto.LastModificationBy.Value
            };

            await _feeRuleRepositoryRepository.Add(feeRule);
            await _feeRuleRepositoryRepository.CommitAsync();
        }

        public async Task CreateByCategory(Guid userId, FeeRuleByCategoryDto feeRuleByCategoryDto)
        {
            var category = await _categoryRepository.GetById(feeRuleByCategoryDto.CategoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with id: {feeRuleByCategoryDto.CategoryId} not found.");
            var products = await _productRepository.Find(x => x.CategoryId == feeRuleByCategoryDto.CategoryId);

            foreach (var prod in products)
            {
                var feeRule = new FeeRule()
                {
                    ProductId = prod.Id,
                    Product = prod,
                    Date = DateTime.Now.ToLocalTime(),
                    FeesAmountTo = feeRuleByCategoryDto.FeesAmountTo,
                    Percentage = feeRuleByCategoryDto.Percentage,
                    CreatedBy = userId
                };

                await _feeRuleRepositoryRepository.Add(feeRule);
            }
            
            await _feeRuleRepositoryRepository.CommitAsync();
        }

        private async Task<List<FeeRule>> GetActiveFeeRules()
        {
            List<FeeRule> feeRules = new List<FeeRule>();
            var feeRulesGroups = (await _feeRuleRepositoryRepository.GetAll(x => x.Product)).GroupBy(x => x.ProductId);
            foreach (var group in feeRulesGroups)
            {
                var subGroups = group.GroupBy(x => x.FeesAmountTo);
                foreach (var subGroup in subGroups)
                {
                    var fee = subGroup.OrderByDescending(x => x.Date).FirstOrDefault();
                    if (fee != null)
                    {
                        feeRules.Add(fee);
                    }
                }
            }

            return feeRules;
        }
    }
}
