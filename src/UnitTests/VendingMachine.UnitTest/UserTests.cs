using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.InterfaceAdapters.Models;
using VendingMachine.InterfaceAdapters.TestApis;

namespace VendingMachine.UnitTest
{
    public class UserTests
    {
        [Fact]
        public async Task Should_Register_User()
        {
            var api = new UserTestApi();

            var input = new UserInputModel
            {
                Username = "ahmed",
                Password = "123456",
                Role = "Buyer",
                Deposit = 0
            };

            var expected = new UserOutputModel
            {
                Id = "u1",
                Username = "ahmed",
                Role = "Buyer",
                Deposit = 0
            };

            var roles = new List<string> { "Buyer" };

            api.SetupRegister(input, expected,roles);

            var result = await api.RegisterAsync(input);

            Assert.Equal(expected.Username, result.Username);
            Assert.Equal(expected.Role, result.Role);
            //Assert.Equal(expected.Token, result.Token);
        }

        [Fact]
        public async Task Should_Login_User()
        {
            var api = new UserTestApi();

            var input = new LoginInputModel
            {
                Username = "ahmed",
                Password = "123456"
            };

            var expected = new UserOutputModel
            {
                Id = "u1",
                Username = "ahmed",
                Role = "Buyer",
                Deposit = 0,
                Token = "token-login"
            };

            var roles = new List<string> { "Buyer" };
            api.SetupLogin(input, expected, roles);

            var result = await api.LoginAsync(input);

            Assert.Equal(expected.Username, result.Username);
            Assert.Equal(expected.Token, result.Token);
        }

        [Fact]
        public async Task Should_Get_User_By_Id()
        {
            var api = new UserTestApi();

            var expected = new UserOutputModel
            {
                Id = "u1",
                Username = "ahmed",
                Role = "Buyer",
                Deposit = 0
            };

            api.SetupGetById("u1", expected);

            var result = await api.GetByIdAsync("u1");

            Assert.NotNull(result);
            Assert.Equal("ahmed", result.Username);
        }

        [Fact]
        public async Task Should_Get_All_Users()
        {
            var api = new UserTestApi();

            var expectedUsers = new List<UserOutputModel>
        {
            new UserOutputModel { Id = "u1", Username = "ahmed", Role = "Buyer", Deposit = 50 },
            new UserOutputModel { Id = "u2", Username = "sara", Role = "Seller", Deposit = 0 }
        };

            api.SetupGetAll(expectedUsers);

            var result = await api.GetAllAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, u => u.Username == "ahmed");
            Assert.Contains(result, u => u.Username == "sara");
        }

        [Fact]
        public async Task Should_Update_User()
        {
            var api = new UserTestApi();

            var input = new UserInputModel
            {
                Username = "updatedUser",
                Password = "newpass",
                Role = "Buyer",
                Deposit = 100
            };

            api.SetupUpdate("u1", input, true);

            var success = await api.UpdateAsync("u1", input);

            Assert.True(success);
        }

        [Fact]
        public async Task Should_Delete_User()
        {
            var api = new UserTestApi();

            api.SetupDelete("u1", true);

            var result = await api.DeleteAsync("u1");

            Assert.True(result);
        }

        [Fact]
        public async Task Should_Update_Deposit()
        {
            var api = new UserTestApi();

            var userId = "u1";
            var newDeposit = 100;

            api.SetupUpdateDeposit(userId);

            var exception = await Record.ExceptionAsync(() => api.UpdateDepositAsync(userId, newDeposit));

            Assert.Null(exception); // if no exception, update worked
        }
    }
}
