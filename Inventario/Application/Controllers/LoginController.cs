using Application.ViewModels;
using AutoMapper;
using Logic.Dtos;
using Logic.Interfaces;
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
    [AllowAnonymous]
    [ApiController]
    [Route("api/login")]
    public class LoginController : Controller
    {
        private readonly IUserUseCases _userUseCases;
        private readonly IConfiguration _configuration;

        public LoginController(IUserUseCases userUseCases, IConfiguration configuration)
        {
            _userUseCases = userUseCases;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            try
            {
                UserDto userDto = await _userUseCases.Login(loginVM.Username, loginVM.Password);
                //IEnumerable<RolDto> roles = _userUseCases.GetRolesUsuarioPorAplicacion(usuario.Id, eAplicaciones.AppWeb);
                //IEnumerable<MenuDto> menues = await _rolesService.GetMenuesFromRoles(roles.Select(r => r.Id));
                //IEnumerable<string> codigosMenues = menues.Select(m => m.Codigo);

                string appUser = loginVM.Username;
                var token = GenerateJwtToken(userDto);
                //return GenerateJwtResult(userDto, roles, codigosMenues, token);
                return Ok(GenerateJwtResult(userDto, token));
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        //private string GenerateJwtToken(UserDto user)
        //{
        //    try
        //    {
        //        var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        //        //new Claim(JwtRegisteredClaimNames.Sub, usuario.NombreApellido),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        //    };

        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //        //var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));
        //        //var expires = DateTime.Now.AddSeconds(Convert.ToDouble(_configuration["JwtExpireSeconds"]));
        //        var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["JwtExpireHours"]));

        //        var token = new JwtSecurityToken(
        //            _configuration["JwtIssuer"],
        //            _configuration["JwtIssuer"],
        //            claims,
        //            expires: expires,
        //            signingCredentials: creds
        //        );

        //        return new JwtSecurityTokenHandler().WriteToken(token);
        //    }
        //    catch(Exception e)
        //    {
        //        throw e;
        //    }
        //}

        private string GenerateJwtToken(UserDto user)
        {
            // CREAMOS EL HEADER //
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            // CREAMOS LOS CLAIMS //
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim("Name", user.Name),
                new Claim("Lastname", user.Lastname),
                new Claim(JwtRegisteredClaimNames.Email, user.Mail),
                new Claim(ClaimTypes.Role, "Rol 1")
            };

            // CREAMOS EL PAYLOAD //
            var _Payload = new JwtPayload(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    // Exipra a la 24 horas.
                    expires: DateTime.UtcNow.AddHours(24)
                );

            // GENERAMOS EL TOKEN //
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);
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
