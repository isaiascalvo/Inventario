using Application.ViewModels;
using AutoMapper;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/miscellaneous-expenses")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MiscellaneousExpensesController : Controller
    {
        private readonly IMiscellaneousExpensesUseCases _miscellaneousExpensesUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public MiscellaneousExpensesController(IMiscellaneousExpensesUseCases miscellaneousExpensesUseCases, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _miscellaneousExpensesUseCases = miscellaneousExpensesUseCases;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<MiscellaneousExpensesDto> miscellaneousExpensesDto = await _miscellaneousExpensesUseCases.GetAll();
            IEnumerable<MiscellaneousExpensesViewModel> categories = _mapper.Map<IEnumerable<MiscellaneousExpensesDto>, IEnumerable<MiscellaneousExpensesViewModel>>(miscellaneousExpensesDto);
            return Ok(categories);
        }

        [HttpGet("{miscellaneousExpensesId}", Name = "GetMiscellaneousExpenses")]
        public async Task<IActionResult> GetOne(Guid miscellaneousExpensesId)
        {
            try
            {
                var miscellaneousExpensesDto = await _miscellaneousExpensesUseCases.GetOne(miscellaneousExpensesId);
                var miscellaneousExpensesVM = _mapper.Map<MiscellaneousExpensesDto, MiscellaneousExpensesViewModel>(miscellaneousExpensesDto);
                return Ok(miscellaneousExpensesVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(MiscellaneousExpensesForCreationDto miscellaneousExpensesForCreationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var miscellaneousExpensesForCreationDto = _mapper.Map<MiscellaneousExpensesForCreationDto, MiscellaneousExpensesForCreationDto>(miscellaneousExpensesForCreationVM);
            var miscellaneousExpensesDto = await _miscellaneousExpensesUseCases.Create(userId, miscellaneousExpensesForCreationDto);
            var miscellaneousExpensesVM = _mapper.Map<MiscellaneousExpensesDto, MiscellaneousExpensesViewModel>(miscellaneousExpensesDto);

            return CreatedAtRoute(
                "GetMiscellaneousExpenses",
                new { miscellaneousExpensesId = miscellaneousExpensesVM.Id },
                miscellaneousExpensesVM
            );
        }

        [HttpDelete("{miscellaneousExpensesId}")]
        public async Task<IActionResult> Delete(Guid miscellaneousExpensesId)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _miscellaneousExpensesUseCases.Delete(userId, miscellaneousExpensesId);
                return Ok("The miscellaneous expense was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{miscellaneousExpensesId}")]
        public async Task<IActionResult> Update(Guid miscellaneousExpensesId, [FromBody] MiscellaneousExpensesViewModel miscellaneousExpenses)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var miscellaneousExpensesDto = _mapper.Map<MiscellaneousExpensesViewModel, MiscellaneousExpensesDto>(miscellaneousExpenses);
                miscellaneousExpensesDto.LastModificationBy = userId;
                await _miscellaneousExpensesUseCases.Update(miscellaneousExpensesId, miscellaneousExpensesDto);
                return Ok("Miscellaneous Expense successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
