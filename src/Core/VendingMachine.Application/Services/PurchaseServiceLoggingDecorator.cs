using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.Purchase;
using VendingMachine.Application.Interfaces;

namespace VendingMachine.Application.Services
{
    public class PurchaseServiceLoggingDecorator : IPurchaseService
    {
        private readonly IPurchaseService _inner;
        private readonly ILogger<PurchaseServiceLoggingDecorator> _logger;

        public PurchaseServiceLoggingDecorator(
            IPurchaseService inner,
            ILogger<PurchaseServiceLoggingDecorator> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task DepositAsync(string userId, int coin)
        {
            _logger.LogInformation("User {UserId} depositing {Coin} cents.", userId, coin);
            await _inner.DepositAsync(userId, coin);
            _logger.LogInformation("User {UserId} deposit completed.", userId);
        }

        public async Task<PurchaseResponseDto> BuyAsync(string userId, int productId, int quantity)
        {
            _logger.LogInformation("User {UserId} attempting to purchase ProductId {ProductId} (Qty: {Quantity})", userId, productId, quantity);
            var result = await _inner.BuyAsync(userId, productId, quantity);
            _logger.LogInformation("User {UserId} completed purchase. Total spent: {Total}, Change returned: {ChangeList}",
                userId, result.TotalSpent, string.Join(", ", result.Change));
            return result;
        }

        public async Task ResetDepositAsync(string userId)
        {
            _logger.LogInformation("User {UserId} resetting deposit.", userId);
            await _inner.ResetDepositAsync(userId);
            _logger.LogInformation("User {UserId} deposit reset completed.", userId);
        }
    }
}
