using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Entities;
using VendingMachine.InterfaceAdapters.Models;
using VendingMachine.InterfaceAdapters.TestApis;

namespace VendingMachine.UnitTest
{
    public class ProductTests
    {
        [Fact]
        public async Task Should_Get_All_Products()
        {
            var api = new ProductTestApi();
            var products = new List<Product>
        {
            new Product { Id = 1, ProductName = "Coke", Cost = 50, AmountAvailable = 10, SellerId = "seller1" },
            new Product { Id = 2, ProductName = "Pepsi", Cost = 70, AmountAvailable = 5, SellerId = "seller2" }
        };

            api.SetupGetAll(products);

            var result = await api.GetAllAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal("Coke", result[0].ProductName);
        }

        [Fact]
        public async Task Should_Get_Product_By_Id()
        {
            var api = new ProductTestApi();
            var product = new Product { Id = 1, ProductName = "Coke", Cost = 50, AmountAvailable = 10, SellerId = "seller1" };

            api.SetupGetById(1, product);

            var result = await api.GetByIdAsync(1);

            Assert.Equal("Coke", result.ProductName);
            Assert.Equal(50, result.Cost);
        }

        [Fact]
        public async Task Should_Add_Product()
        {
            var api = new ProductTestApi();
            var input = new ProductInputModel
            {
                Id = 3,
                ProductName = "Fanta",
                Cost = 60,
                AmountAvailable = 7,
                SellerId = "seller3"
            };

            api.SetupAdd(input);

            await api.AddAsync(input, "seller3");
        }

        [Fact]
        public async Task Should_Update_Product()
        {
            var api = new ProductTestApi();
            var input = new ProductInputModel
            {
                Id = 1,
                ProductName = "Sprite",
                Cost = 40,
                AmountAvailable = 12,
                SellerId = "seller1"
            };

            api.SetupUpdate();

            await api.UpdateAsync(1,input, "seller3");
        }

        [Fact]
        public async Task Should_Delete_Product()
        {
            var api = new ProductTestApi();

            api.SetupDelete();

            await api.DeleteAsync(1, "seller3");
        }
    }
}
