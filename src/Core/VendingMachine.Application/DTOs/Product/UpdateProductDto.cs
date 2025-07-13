using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Application.DTOs.Product
{
    public class UpdateProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Cost { get; set; }
        public int AmountAvailable { get; set; }
    }
}
