using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.InterfaceAdapters.Models
{
    public class ProductInputModel
    {
        public string ProductName { get; set; }
        public int Cost { get; set; }
        public int AmountAvailable { get; set; }
        public string SellerId { get; set; }
        public int? Id { get; set; } // for update/delete
    }
}
