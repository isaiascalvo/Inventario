using AutoMapper;
using Data;
using Infrastructure.Repositories;
using Logic.Dtos;
using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Util.Functions;

namespace Logic
{
    public class UserUseCases : IUserUseCases
    {
        private readonly IUserRepository _userRepository;
        private readonly ISendMailUseCases _sendMailUseCases;
        private readonly IMapper _mapper;

        public UserUseCases(IUserRepository userRepository, ISendMailUseCases sendMailUseCases, IMapper mapper)
        {
            _userRepository = userRepository;
            _sendMailUseCases = sendMailUseCases;
            _mapper = mapper;
        }

        public async Task<UserDto> Create(Guid userId, UserForCreationDto userForCreationDto)
        {
            var hasedPassword = CommonFunctions.MD5(userForCreationDto.Password);
            var user = new User()
            {
                Username = userForCreationDto.Username,
                Password = hasedPassword,
                Name = userForCreationDto.Name,
                Lastname = userForCreationDto.Lastname,
                Dni = userForCreationDto.Dni,
                Phone = userForCreationDto.Phone,
                Mail = userForCreationDto.Mail,
                IsAdmin = userForCreationDto.IsAdmin,
                Active = userForCreationDto.Active,
                CreatedBy = userId
            };

            user = await _userRepository.Add(user);
            await _userRepository.CommitAsync();
            return _mapper.Map<User, UserDto>(user);
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var user = await _userRepository.Delete(userId, id);
            if (user == null)
                throw new KeyNotFoundException($"User with id: {id} not found.");

            await _userRepository.CommitAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var users = (await _userRepository.GetAll()).Select(x => new User {
                Id = x.Id,
                Username = x.Username,
                Password = null,
                Name = x.Name,
                Lastname = x.Lastname,
                Dni = x.Dni,
                Phone = x.Phone,
                Mail = x.Mail,
                IsAdmin = x.IsAdmin,
                Active = x.Active,
                CreatedAt = x.CreatedAt,
                LastModificationAt = x.LastModificationAt,
                IsDeleted = x.IsDeleted,
                DeletedAt = x.DeletedAt,

            });
            var usersDto = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
            return usersDto;
        }

        public async Task<UserDto> GetOne(Guid id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                throw new KeyNotFoundException($"User with id: {id} not found.");
            user.Password = null;

            return _mapper.Map<User, UserDto>(user);
        }

        public async Task Update(Guid id, UserDto userDto)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                throw new KeyNotFoundException($"User with id: {id} not found.");

            //user.Username = userForCreationDto.Username;
            //user.Password = userForCreationDto.Password;
            user.Name = userDto.Name;
            user.Lastname = userDto.Lastname;
            user.Dni = userDto.Dni;
            user.Phone = userDto.Phone;
            user.Mail = userDto.Mail;
            user.IsAdmin = userDto.IsAdmin;
            user.Active = userDto.Active;
            user.LastModificationBy = userDto.LastModificationBy;

            await _userRepository.Update(user);
            await _userRepository.CommitAsync();
        }

        public async Task ChangePassword(Guid id, string actualPassword, string newPassword)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                throw new KeyNotFoundException($"User with id: {id} not found.");
            if (user.Password != CommonFunctions.MD5(actualPassword))
                throw new AuthenticationException($"The current password entered is not correct.");

            user.Password = CommonFunctions.MD5(newPassword);
            await _userRepository.Update(user);
            await _userRepository.CommitAsync();
        }
        public async Task ResetPassword(string mail)
        {
            var user = (await _userRepository.Find(x => x.Mail == mail)).FirstOrDefault();
            if (user == null)
                throw new KeyNotFoundException($"User with mail: {mail} not found.");

            var newPassword = Guid.NewGuid().ToString().Substring(10);
            user.Password = CommonFunctions.MD5(newPassword);
            await _userRepository.Update(user);
            await _userRepository.CommitAsync();

            _sendMailUseCases.SendMail(
                user.Mail,
                "Reseteo de contraseña",
                "Su nueva contraseña es " + newPassword + ". Le recomendamos que la utilice para ingresar al sistema y cambiarla por una nueva."
            );
        }

        public async Task<UserDto> Login(string userName, string password)
        {
            var passHased = CommonFunctions.MD5(password);
            var user = (await _userRepository.Find(x => x.Username == userName && x.Password == passHased && x.Active)).FirstOrDefault();
            if (user == null)
                throw new AuthenticationException($"Error.");

            return _mapper.Map<User, UserDto>(user);
        }
    }
}
