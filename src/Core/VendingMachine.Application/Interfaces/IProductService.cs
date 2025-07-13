using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.Product;

namespace VendingMachine.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto dto, string sellerId);
        Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto, string sellerId);
        Task<bool> DeleteAsync(int id, string sellerId);
    }
}
