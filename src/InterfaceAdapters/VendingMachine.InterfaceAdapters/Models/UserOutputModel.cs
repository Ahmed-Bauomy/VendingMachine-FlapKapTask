using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.InterfaceAdapters.Models
{
    public class UserOutputModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int Deposit { get; set; }
        public string Token { get; set; }
    }
}
