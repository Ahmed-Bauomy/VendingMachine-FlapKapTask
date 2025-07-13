using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.User;
using VendingMachine.Domain.Entities;
using VendingMachine.InterfaceAdapters.Models;
using VendingMachine.InterfaceAdapters.TestApis;

namespace VendingMachine.UnitTest
{
    public class PurchaseTests
    {
        [Fact]
        public async Task Should_Deposit_Coins()
        {
            // Arrange
            var api = new PurchaseTestApi();
            var input = new DepositInputModel { UserId = "user1", Coin = 50 };
            var user = new UserDto { Id = "user1", Deposit = 0 };

            api.SetupGetUserById("user1", user);
            api.SetupUpdateUser("user1");

            // Act
            await api.DepositAsync(input);

            // Assert: no exception means success (optionally verify update was called)
        }

        [Fact]
        public async Task Should_Buy_Product()
        {
            // Arrange
            var api = new PurchaseTestApi();
            var input = new PurchaseInputModel
            {
                UserId = "user1",
                ProductId = 1,
                Quantity = 2
            };

            var user = new UserDto
            {
                Id = "user1",
                Deposit = 200
            };

            var product = new Product
            {
                Id = 1,
                ProductName = "Pepsi",
                Cost = 50,
                AmountAvailable = 5,
                SellerId = "seller1"
            };

            api.SetupGetUserById("user1", user);
            api.SetupUpdateUser("user1");
            api.SetupGetProductById(1, product);
            api.SetupUpdateProduct();

            // Act
            var result = await api.BuyAsync(input);

            // Assert
            Assert.Equal("Pepsi", result.ProductName);
            Assert.Equal(100, result.TotalSpent);
            Assert.Equal(new List<int> { 100 }, result.Change);
        }

        [Fact]
        public async Task Should_Reset_Deposit()
        {
            // Arrange
            var api = new PurchaseTestApi();
            var input = new ResetInputModel { UserId = "user1" };
            var user = new UserDto { Id = "user1", Deposit = 150 };

            api.SetupGetUserById("user1", user);
            api.SetupUpdateUser("user1");

            // Act
            await api.ResetDepositAsync(input);

            // Assert: again, no exception means it passed
        }
    }
}
