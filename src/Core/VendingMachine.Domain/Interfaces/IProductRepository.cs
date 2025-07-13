using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Entities;

namespace VendingMachine.Domain.Interfaces
{
    public interface IProductRepository : IAsyncRepository<Product>
    {

    }
}
