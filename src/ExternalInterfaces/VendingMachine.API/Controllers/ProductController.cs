using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VendingMachine.Application.DTOs.Product;
using VendingMachine.Application.Interfaces;
using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Enums;

namespace VendingMachine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (sellerId == null) return Unauthorized();

            var created = await _productService.CreateAsync(dto, sellerId);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _productService.GetByIdAsync(id);
            if (sellerId == null || product.SellerId != sellerId) return Unauthorized();

            var updated = await _productService.UpdateAsync(id, dto, sellerId);
            if (updated == null) return Forbid();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Delete(int id)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _productService.GetByIdAsync(id);
            if (sellerId == null || product.SellerId != sellerId) return Unauthorized();

            var deleted = await _productService.DeleteAsync(id, sellerId);
            if (!deleted) return Forbid();

            return NoContent();
        }
    }
}
