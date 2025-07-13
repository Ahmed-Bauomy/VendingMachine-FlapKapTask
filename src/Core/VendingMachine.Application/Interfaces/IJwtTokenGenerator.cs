using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.User;

namespace VendingMachine.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserDto user, IList<string> roles);
    }
}
