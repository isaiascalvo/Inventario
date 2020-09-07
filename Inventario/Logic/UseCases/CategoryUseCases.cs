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
    public class CategoryUseCases : ICategoryUseCases
    {
        private readonly ICategoryRepository _categoryRepositoryRepository;
        private readonly IMapper _mapper;

        public CategoryUseCases(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepositoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Create(Guid userId, CategoryForCreationDto categoryForCreationDto)
        {
            var category = new Data.Category()
            {
                Name = categoryForCreationDto.Name,
                Description = categoryForCreationDto.Description,
                CreatedBy = userId
            };

            category = await _categoryRepositoryRepository.Add(category);
            await _categoryRepositoryRepository.CommitAsync();
            return _mapper.Map<Data.Category, CategoryDto>(category);
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var category = await _categoryRepositoryRepository.Delete(userId, id);
            if (category == null)
                throw new KeyNotFoundException($"Category with id: {id} not found.");

            await _categoryRepositoryRepository.CommitAsync();
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = (await _categoryRepositoryRepository.GetAll()).OrderBy(x => x.Name);
            var categoriesDto = _mapper.Map<IEnumerable<Data.Category>, IEnumerable<CategoryDto>>(categories);
            return categoriesDto;
        }

        public async Task<int> GetTotalQty()
        {
            return (await _categoryRepositoryRepository.GetAll()).Count();
        }

        public async Task<IEnumerable<CategoryDto>> GetByPageAndQty(int skip, int qty)
        {
            var categories = (await _categoryRepositoryRepository.GetAll()).OrderByDescending(x => x.Name).Skip(skip).Take(qty);
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetOne(Guid id)
        {
            var category = await _categoryRepositoryRepository.GetById(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with id: {id} not found.");

            return _mapper.Map<Data.Category, CategoryDto>(category);
        }

        public async Task Update(Guid id, CategoryDto categoryDto)
        {
            var category = await _categoryRepositoryRepository.GetById(id);
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            category.LastModificationBy = categoryDto.LastModificationBy;
            await _categoryRepositoryRepository.Update(category);
            await _categoryRepositoryRepository.CommitAsync();
        }
    }
}
