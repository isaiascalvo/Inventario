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
        Task Delete(Guid userId, Guid id);
        Task<UserDto> Create(Guid userId, UserForCreationDto userDTO);
        Task Update(Guid id, UserDto USERdTO);
        Task<UserDto> Login(string userName, string password);
        Task<int> GetTotalQty();
        Task<IEnumerable<UserDto>> GetByPageAndQty(int skip, int qty);
        Task<int> GetTotalQtyByFilters(UserFiltersDto filtersDto);
        Task<IEnumerable<UserDto>> GetFilteredByPageAndQty(UserFiltersDto filtersDto, int skip, int qty);
    }
}
