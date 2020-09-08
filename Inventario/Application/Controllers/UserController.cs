using Application.ViewModels;
using AutoMapper;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly IUserUseCases _userUseCases;
        private readonly ISendMailUseCases _sendMailUseCases;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(IUserUseCases userUseCases, ISendMailUseCases sendMailUseCases, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IMapper mapper)
        {
            _userUseCases = userUseCases;
            _sendMailUseCases = sendMailUseCases;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<UserDto> usersDto = await _userUseCases.GetAll();
            IEnumerable<UserViewModel> usersVM = _mapper.Map<IEnumerable<UserDto>, IEnumerable<UserViewModel>>(usersDto);
            return Ok(usersVM);
        }

        [HttpGet("GetTotalQty")]
        public async Task<IActionResult> GetTotalQty()
        {
            try
            {
                int qty = await _userUseCases.GetTotalQty();
                return Ok(qty);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetByPageAndQty")]
        public async Task<IActionResult> GetByPageAndQty(int skip, int qty)
        {
            try
            {
                IEnumerable<UserDto> usersDto = await _userUseCases.GetByPageAndQty(skip, qty);
                IEnumerable<UserViewModel> usersVM = _mapper.Map<IEnumerable<UserDto>, IEnumerable<UserViewModel>>(usersDto);
                return Ok(usersVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpGet("GetTotalQtyByFilters")]
        public async Task<IActionResult> GetTotalQtyByFilters([FromQuery] UserFiltersViewModel filtersVM)
        {
            try
            {
                var filtersDto = _mapper.Map<UserFiltersViewModel, UserFiltersDto>(filtersVM);
                int qty = await _userUseCases.GetTotalQtyByFilters(filtersDto);
                return Ok(qty);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetByFiltersPageAndQty")]
        public async Task<IActionResult> GetByFiltersPageAndQty([FromQuery] UserFiltersViewModel filtersVM, int skip, int qty)
        {
            try
            {
                var filtersDto = _mapper.Map<UserFiltersViewModel, UserFiltersDto>(filtersVM);
                IEnumerable<UserDto> usersDto = await _userUseCases.GetFilteredByPageAndQty(filtersDto, skip, qty);
                IEnumerable<UserViewModel> usersVM = _mapper.Map<IEnumerable<UserDto>, IEnumerable<UserViewModel>>(usersDto);
                return Ok(usersVM);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetOne(Guid userId)
        {
            try
            {
                var userDto = await _userUseCases.GetOne(userId);
                var userVM = _mapper.Map<UserDto, UserViewModel>(userDto);
                return Ok(userVM);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserForCreationViewModel userForCreationVM)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

            var userForCreationDto = _mapper.Map<UserForCreationViewModel, UserForCreationDto>(userForCreationVM);
            var userDto = await _userUseCases.Create(userId, userForCreationDto);
            var userVM = _mapper.Map<UserDto, UserViewModel>(userDto);

            return CreatedAtRoute(
                "GetUser",
                new { userId = userVM.Id },
                userVM
            );
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            try
            {
                var deletor = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                await _userUseCases.Delete(deletor, userId);
                return Ok("The user was deleted.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(Guid userId, [FromBody] UserViewModel userVM)
        {
            try
            {
                var modifier = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("NameId"));

                var userDto = _mapper.Map<UserViewModel, UserDto>(userVM);
                userDto.LastModificationBy = modifier;
                await _userUseCases.Update(userId, userDto);
                return Ok("User successfully updated.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
