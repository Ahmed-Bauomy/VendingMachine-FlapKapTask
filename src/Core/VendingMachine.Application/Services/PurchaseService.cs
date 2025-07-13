using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.Purchase;
using VendingMachine.Application.Interfaces;
using VendingMachine.Domain.Interfaces;

namespace VendingMachine.Application.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IUserService _userService;
        private readonly IProductRepository _productRepository;
        private readonly List<int> _validCoins = new() { 5, 10, 20, 50, 100 };

        public PurchaseService(IUserService userService, IProductRepository productRepository)
        {
            _userService = userService;
            _productRepository = productRepository;
        }

        public async Task DepositAsync(string userId, int coin)
        {
            if (!_validCoins.Contains(coin))
                throw new ArgumentException("Invalid coin");

            var user = await _userService.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var newDeposit = user.Deposit + coin;
            await _userService.UpdateDepositAsync(userId, newDeposit);
        }

        public async Task<PurchaseResponseDto> BuyAsync(string userId, int productId, int quantity)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) throw new Exception("Product not found");

            if (product.AmountAvailable < quantity)
                throw new InvalidOperationException("Not enough product in stock");

            var totalCost = product.Cost * quantity;

            if (user.Deposit < totalCost)
                throw new InvalidOperationException("Insufficient deposit");

            product.AmountAvailable -= quantity;
            await _productRepository.UpdateAsync(product);

            var remaining = user.Deposit - totalCost;
            var change = CalculateChange(remaining);

            //await _userService.UpdateDepositAsync(userId, 0); // reset to 0 after change returned

            return new PurchaseResponseDto
            {
                ProductName = product.ProductName,
                Quantity = quantity,
                TotalSpent = totalCost,
                Change = change
            };
        }

        public async Task ResetDepositAsync(string userId)
        {
            await _userService.UpdateDepositAsync(userId, 0);
        }

        private List<int> CalculateChange(int changeAmount)
        {
            var change = new List<int>();

            var coins = _validCoins
                .Where(c => c <= changeAmount)     
                .OrderByDescending(c => c)
                .ToList();

            foreach (var coin in coins)
            {
                while (changeAmount >= coin)
                {
                    change.Add(coin);
                    changeAmount -= coin;
                }
                if (changeAmount == 0) break;
            }

            return change;
        }
    }
}
