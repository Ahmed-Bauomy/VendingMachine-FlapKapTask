using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.User;

namespace VendingMachine.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterUserDto dto);
        Task<UserDto?> LoginAsync(LoginUserDto dto);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(string id);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(string id, RegisterUserDto dto); // reuse Register DTO
        Task UpdateDepositAsync(string userId, int newDeposit);
    }
}
