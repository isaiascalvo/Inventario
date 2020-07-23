using Application.ViewModels;
using AutoMapper;
using Logic.Dtos;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(IUserUseCases userUseCases, ISendMailUseCases sendMailUseCases, IConfiguration configuration, IMapper mapper)
        {
            _userUseCases = userUseCases;
            _sendMailUseCases = sendMailUseCases;
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

        [HttpPost("Login")]
        public async Task<object> Login(LoginViewModel loginVM)
        {
            UserDto userDto = await _userUseCases.Login(loginVM.Username, loginVM.Password);
            //IEnumerable<RolDto> roles = _userUseCases.GetRolesUsuarioPorAplicacion(usuario.Id, eAplicaciones.AppWeb);
            //IEnumerable<MenuDto> menues = await _rolesService.GetMenuesFromRoles(roles.Select(r => r.Id));
            //IEnumerable<string> codigosMenues = menues.Select(m => m.Codigo);

            string appUser = loginVM.Username;
            var token = GenerateJwtToken(userDto);
            //return GenerateJwtResult(userDto, roles, codigosMenues, token);
            return GenerateJwtResult(userDto, token);
        }

        private string GenerateJwtToken(UserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                //new Claim(JwtRegisteredClaimNames.Sub, usuario.NombreApellido),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));
            //var expires = DateTime.Now.AddSeconds(Convert.ToDouble(_configuration["JwtExpireSeconds"]));
            var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["JwtExpireHours"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtResultViewModel GenerateJwtResult(
            UserDto userResult,
            //IEnumerable<RolDto> roles,
            //IEnumerable<string> codigosMenues,
            string token)
        {
            return new JwtResultViewModel
            {
                Id = userResult.Id,
                Mail = userResult.Mail,
                Name = userResult.Name,
                Lastname = userResult.Lastname,
                Token = token,
                //Roles = roles.Select(r => r.Codigo),
                //CodigosMenues = codigosMenues,
                Username = userResult.Username
            };
        }
    }
}
