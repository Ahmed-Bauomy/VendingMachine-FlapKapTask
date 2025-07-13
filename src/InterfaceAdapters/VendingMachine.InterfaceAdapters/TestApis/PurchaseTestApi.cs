using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.Purchase;
using VendingMachine.Application.DTOs.User;
using VendingMachine.Application.Interfaces;
using VendingMachine.Application.Services;
using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Interfaces;
using VendingMachine.InterfaceAdapters.Models;

namespace VendingMachine.InterfaceAdapters.TestApis
{
    public class PurchaseTestApi
    {
        private readonly PurchaseService _service;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly IMapper _mapper;

        public PurchaseTestApi()
        {
            _userServiceMock = new Mock<IUserService>();
            _productRepoMock = new Mock<IProductRepository>();

            _service = new PurchaseService(_userServiceMock.Object, _productRepoMock.Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseResponseDto, PurchaseOutputModel>().ReverseMap();
            });

            _mapper = config.CreateMapper();
        }

        // -------- Setup Methods (Mocks) --------
        public void SetupGetUserById(string userId, UserDto user)
        {
            _userServiceMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);
        }

        public void SetupUpdateUser(string userId)
        {
            _userServiceMock.Setup(s => s.UpdateAsync(userId,It.IsAny<RegisterUserDto>())).Returns(Task.FromResult(true));
        }

        public void SetupGetProductById(int productId, Product product)
        {
            _productRepoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
        }

        public void SetupUpdateProduct()
        {
            _productRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
        }

        // -------- Business Methods --------
        public async Task DepositAsync(DepositInputModel input)
        {
            await _service.DepositAsync(input.UserId, input.Coin);
        }

        public async Task ResetDepositAsync(ResetInputModel input)
        {
            await _service.ResetDepositAsync(input.UserId);
        }

        public async Task<PurchaseOutputModel> BuyAsync(PurchaseInputModel input)
        {
            var result = await _service.BuyAsync(input.UserId, input.ProductId, input.Quantity);
            return _mapper.Map<PurchaseOutputModel>(result);
        }
    }
}
