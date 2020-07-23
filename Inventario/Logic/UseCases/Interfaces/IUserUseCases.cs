using Logic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IUserUseCases
    {
        Task<UserDto> GetOne(Guid id);
        Task<IEnumerable<UserDto>> GetAll();
        Task Delete(Guid id);
        Task<UserDto> Create(UserForCreationDto userDTO);
        Task Update(Guid id, UserDto USERdTO);
        Task<UserDto> Login(string userName, string password);
    }
}
