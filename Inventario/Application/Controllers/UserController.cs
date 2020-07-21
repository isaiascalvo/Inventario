using Application.ViewModels;
using AutoMapper;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserUseCases _userUseCases;
        private readonly ISendMailUseCases _sendMailUseCases;
        private readonly IMapper _mapper;

        public UserController(IUserUseCases userUseCases, ISendMailUseCases sendMailUseCases, IMapper mapper)
        {
            _userUseCases = userUseCases;
            _sendMailUseCases = sendMailUseCases;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<UserDto> usersDto = await _userUseCases.GetAll();
            IEnumerable<UserViewModel> usersVM = _mapper.Map<IEnumerable<UserDto>, IEnumerable<UserViewModel>>(usersDto);
            return Ok(usersVM);
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
            var userForCreationDto = _mapper.Map<UserForCreationViewModel, UserForCreationDto>(userForCreationVM);
            var userDto = await _userUseCases.Create(userForCreationDto);
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
                await _userUseCases.Delete(userId);
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
                var userDto = _mapper.Map<UserViewModel, UserDto>(userVM);
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
