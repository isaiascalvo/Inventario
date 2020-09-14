using AutoMapper;
using Data;
using Data.Models;
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
    public class MiscellaneousExpensesUseCases : IMiscellaneousExpensesUseCases
    {
        private readonly IMiscellaneousExpensesRepository _miscellaneousExpensesRepository;
        private readonly IMapper _mapper;

        public MiscellaneousExpensesUseCases(IMiscellaneousExpensesRepository miscellaneousExpensesRepository, IMapper mapper)
        {
            _miscellaneousExpensesRepository = miscellaneousExpensesRepository;
            _mapper = mapper;
        }

        public async Task<MiscellaneousExpensesDto> Create(Guid userId, MiscellaneousExpensesForCreationDto miscellaneousExpensesForCreationDto)
        {
            var miscellaneousExpenses = new MiscellaneousExpenses()
            {
                Description = miscellaneousExpensesForCreationDto.Description,
                Date = miscellaneousExpensesForCreationDto.Date.ToLocalTime(),
                Value = miscellaneousExpensesForCreationDto.Value,
                Destination = miscellaneousExpensesForCreationDto.Destination,
                IsFixed = miscellaneousExpensesForCreationDto.IsFixed,
                CreatedBy = userId
            };

            miscellaneousExpenses = await _miscellaneousExpensesRepository.Add(miscellaneousExpenses);
            await _miscellaneousExpensesRepository.CommitAsync();
            return _mapper.Map<MiscellaneousExpenses, MiscellaneousExpensesDto>(miscellaneousExpenses);
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var miscellaneousExpense = await _miscellaneousExpensesRepository.Delete(userId, id);
            if (miscellaneousExpense == null)
                throw new KeyNotFoundException($"Miscellaneous Expense with id: {id} not found.");

            await _miscellaneousExpensesRepository.CommitAsync();
        }

        public async Task<IEnumerable<MiscellaneousExpensesDto>> GetAll()
        {
            var miscellaneousExpenses = (await _miscellaneousExpensesRepository.GetAll()).OrderBy(x => x.Date);
            var miscellaneousExpensesDto = _mapper.Map<IEnumerable<MiscellaneousExpenses>, IEnumerable<MiscellaneousExpensesDto>>(miscellaneousExpenses);
            return miscellaneousExpensesDto;
        }
        public async Task<int> GetTotalQty()
        {
            return (await _miscellaneousExpensesRepository.GetAll()).Count();
        }

        public async Task<int> GetTotalQtyByFilters(MiscellaneousExpensesFiltersDto filtersDto)
        {
            return (await _miscellaneousExpensesRepository.GetAll()).AsQueryable().Where(filtersDto.GetExpresion()).Count();
        }

        public async Task<IEnumerable<MiscellaneousExpensesDto>> GetByPageAndQty(int skip, int qty)
        {
            var miscellaneousExpenses = (await _miscellaneousExpensesRepository.GetAll()).OrderByDescending(x => x.Date).ThenBy(x => x.Description).Skip(skip).Take(qty);
            return _mapper.Map<IEnumerable<MiscellaneousExpenses>, IEnumerable<MiscellaneousExpensesDto>>(miscellaneousExpenses);
        }

        public async Task<IEnumerable<MiscellaneousExpensesDto>> GetFilteredByPageAndQty(MiscellaneousExpensesFiltersDto filtersDto, int skip, int qty)
        {
            var miscellaneousExpenses = (await _miscellaneousExpensesRepository.GetAll())
                .AsQueryable().Where(filtersDto.GetExpresion()).ToList().OrderByDescending(x => x.Date).ThenBy(x => x.Description).Skip(skip).Take(qty);
            return _mapper.Map<IEnumerable<MiscellaneousExpenses>, IEnumerable<MiscellaneousExpensesDto>>(miscellaneousExpenses);
        }

        public async Task<MiscellaneousExpensesDto> GetOne(Guid id)
        {
            var miscellaneousExpense = await _miscellaneousExpensesRepository.GetById(id);
            if (miscellaneousExpense == null)
                throw new KeyNotFoundException($"Miscellaneous Expense with id: {id} not found.");

            return _mapper.Map<MiscellaneousExpenses, MiscellaneousExpensesDto>(miscellaneousExpense);
        }

        public async Task Update(Guid id, MiscellaneousExpensesDto miscellaneousExpenseDto)
        {
            var miscellaneousExpense = await _miscellaneousExpensesRepository.GetById(id);
            miscellaneousExpense.Description = miscellaneousExpenseDto.Description;
            miscellaneousExpense.Date = miscellaneousExpenseDto.Date.ToLocalTime();
            miscellaneousExpense.Value = miscellaneousExpenseDto.Value;
            miscellaneousExpense.Destination = miscellaneousExpenseDto.Destination;
            miscellaneousExpense.IsFixed = miscellaneousExpenseDto.IsFixed;
            miscellaneousExpense.LastModificationBy = miscellaneousExpenseDto.LastModificationBy;
            await _miscellaneousExpensesRepository.Update(miscellaneousExpense);
            await _miscellaneousExpensesRepository.CommitAsync();
        }
    }
}
