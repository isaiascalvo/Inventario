using Application.ViewModels;
using AutoMapper;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : Controller
    {
        private readonly ICategoryUseCases _categoryUseCases;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryUseCases categoryUseCases, IMapper mapper)
        {
            _categoryUseCases = categoryUseCases;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CategoryDto> categoriesDto = await _categoryUseCases.GetAll();
            IEnumerable<CategoryViewModel> categories = _mapper.Map<IEnumerable<CategoryDto>, IEnumerable<CategoryViewModel>>(categoriesDto);
            return Ok(categories);
        }

        [HttpGet("{categoryId}", Name = "GetCategory")]
        public async Task<IActionResult> GetOne(Guid categoryId)
        {
            try
            {
                var categoryDto = await _categoryUseCases.GetOne(categoryId);
                var categoryVM = _mapper.Map<CategoryDto, CategoryViewModel>(categoryDto);
                return Ok(categoryVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryForCreationViewModel categoryForCreationVM)
        {
            var categoryForCreationDto = _mapper.Map<CategoryForCreationViewModel, CategoryForCreationDto>(categoryForCreationVM);
            var categoryDto = await _categoryUseCases.Create(categoryForCreationDto);
            var categoryVM = _mapper.Map<CategoryDto, CategoryViewModel>(categoryDto);

            return CreatedAtRoute(
                "GetCategory",
                new { categoryId = categoryVM.Id },
                categoryVM
            );
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            try
            {
                await _categoryUseCases.Delete(categoryId);
                return Ok("The category was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update(Guid categoryId, [FromBody] CategoryViewModel category)
        {
            try
            {
                var categoryDto = _mapper.Map<CategoryViewModel, CategoryDto>(category);
                await _categoryUseCases.Update(categoryId, categoryDto);
                return Ok("Category successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
