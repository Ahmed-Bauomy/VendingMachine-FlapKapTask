using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.Product;
using VendingMachine.Application.Interfaces;

namespace VendingMachine.Application.Services
{
    public class ProductServiceLoggingDecorator : IProductService
    {
        private readonly IProductService _inner;
        private readonly ILogger<ProductServiceLoggingDecorator> _logger;

        public ProductServiceLoggingDecorator(
            IProductService inner,
            ILogger<ProductServiceLoggingDecorator> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto, string sellerId)
        {
            _logger.LogInformation("Creating product: {ProductName}", dto.ProductName);
            var result = await _inner.CreateAsync(dto,sellerId);
            _logger.LogInformation("Product created: {ProductId}", result.Id);
            return result;
        }

        public async Task<bool> DeleteAsync(int id, string sellerId)
        {
            _logger.LogInformation("Deleting product with ID: {ProductId}", id);
            var result = await _inner.DeleteAsync(id,sellerId);
            if(result) _logger.LogInformation("Deleted product with ID: {ProductId}", id);
            return result;

        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all products.");
            var result = await _inner.GetAllAsync();
            _logger.LogInformation("Retrieved {Count} products.", result.Count());
            return result;
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving product with ID: {ProductId}", id);
            var result = await _inner.GetByIdAsync(id);
            if (result == null)
                _logger.LogWarning("Product with ID {ProductId} not found.", id);
            else
                _logger.LogInformation("Product found: {ProductName}", result.ProductName);

            return result;
        }

        public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto, string sellerId)
        {
            _logger.LogInformation("Updating product with ID: {ProductId}", id);
            var result = await _inner.UpdateAsync(id, dto,sellerId);
            _logger.LogInformation("Updated product: {ProductName}", result.ProductName);
            return result;
        }
    }
}
