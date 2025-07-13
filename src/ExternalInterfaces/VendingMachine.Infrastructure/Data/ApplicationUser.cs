using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Entities;

namespace VendingMachine.Infrastructure.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int Deposit { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
