using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VendingMachine.Application.DTOs.Purchase;
using VendingMachine.Application.Interfaces;

namespace VendingMachine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Buyer")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _buyerService;
        private readonly IUserService _userService;

        public PurchaseController(IPurchaseService buyerService, IUserService userService)
        {
            _buyerService = buyerService;
            _userService = userService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _buyerService.DepositAsync(userId!, dto.Coin);
            return NoContent();
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy([FromBody] PurchaseRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _buyerService.BuyAsync(userId!, dto.ProductId, dto.Quantity);
            return Ok(result);
        }

        [HttpPost("reset")]
        public async Task<IActionResult> Reset()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _buyerService.ResetDepositAsync(userId!);
            return NoContent();
        }
    }
}
