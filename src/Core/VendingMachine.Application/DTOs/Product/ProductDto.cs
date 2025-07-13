using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Application.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Cost { get; set; }
        public int AmountAvailable { get; set; }
        public string SellerId { get; set; } = string.Empty;
    }
}
