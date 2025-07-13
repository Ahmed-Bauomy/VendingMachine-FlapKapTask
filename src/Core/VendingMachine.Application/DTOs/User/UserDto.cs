using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Application.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int Deposit { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
        public string Token { get; set; } = string.Empty; // only for login
    }
}
