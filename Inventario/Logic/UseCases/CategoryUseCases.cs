using AutoMapper;
using Data;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class CategoryUseCases : ICategoryUseCases
    {
        private readonly ICategoryRepository __categoryRepositoryRepository;
        private readonly IMapper _mapper;

        public CategoryUseCases(ICategoryRepository categoryRepository, IMapper mapper)
        {
            __categoryRepositoryRepository = categoryRepository;
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

            return _mapper.Map<Data.Category, CategoryDto>(await __categoryRepositoryRepository.Add(category));
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var category = await __categoryRepositoryRepository.Delete(userId, id);
            if (category == null)
                throw new KeyNotFoundException($"Category with id: {id} not found.");
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = await __categoryRepositoryRepository.GetAll();
            var categoriesDto = _mapper.Map<IEnumerable<Data.Category>, IEnumerable<CategoryDto>>(categories);
            return categoriesDto;
        }

        public async Task<CategoryDto> GetOne(Guid id)
        {
            var category = await __categoryRepositoryRepository.GetById(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with id: {id} not found.");

            return _mapper.Map<Data.Category, CategoryDto>(category);
        }

        public async Task Update(Guid id, CategoryDto categoryDto)
        {
            var category = await __categoryRepositoryRepository.GetById(id);
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            category.LastModificationBy = categoryDto.LastModificationBy;
            await __categoryRepositoryRepository.Update(category);
        }
    }
}
