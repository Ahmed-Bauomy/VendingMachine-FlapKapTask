using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.Product;
using VendingMachine.Application.Interfaces;
using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Interfaces;

namespace VendingMachine.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository productRepository,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Cost = p.Cost,
                AmountAvailable = p.AmountAvailable,
                SellerId = p.SellerId
            });
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Cost = product.Cost,
                AmountAvailable = product.AmountAvailable,
                SellerId = product.SellerId
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto, string sellerId)
        {
            var product = new Product
            {
                ProductName = dto.ProductName,
                Cost = dto.Cost,
                AmountAvailable = dto.AmountAvailable,
                SellerId = sellerId
            };

            await _productRepository.AddAsync(product);

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Cost = product.Cost,
                AmountAvailable = product.AmountAvailable,
                SellerId = product.SellerId
            };
        }

        public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto, string sellerId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.SellerId != sellerId)
                return null; // Unauthorized or not found

            product.ProductName = dto.ProductName;
            product.Cost = dto.Cost;
            product.AmountAvailable = dto.AmountAvailable;

            await _productRepository.UpdateAsync(product);

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Cost = product.Cost,
                AmountAvailable = product.AmountAvailable,
                SellerId = product.SellerId
            };
        }

        public async Task<bool> DeleteAsync(int id, string sellerId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.SellerId != sellerId)
                return false;

            await _productRepository.DeleteAsync(product);
            return true;
        }
    }
}
