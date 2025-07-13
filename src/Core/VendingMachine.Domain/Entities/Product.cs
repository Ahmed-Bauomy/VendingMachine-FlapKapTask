using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Cost { get; set; } // In cents (must be multiple of 5)

        public int AmountAvailable { get; set; }

        public string SellerId { get; set; } = string.Empty; // FK to Identity User
    }
}
