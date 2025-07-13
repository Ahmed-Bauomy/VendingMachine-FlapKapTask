using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.InterfaceAdapters.Models
{
    public class DepositInputModel
    {
        public string UserId { get; set; }
        public int Coin { get; set; }
    }

}
