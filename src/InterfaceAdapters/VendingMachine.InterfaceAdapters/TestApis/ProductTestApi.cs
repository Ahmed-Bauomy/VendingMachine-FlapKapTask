using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.Product;
using VendingMachine.Application.Interfaces;
using VendingMachine.Application.Services;
using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Interfaces;
using VendingMachine.InterfaceAdapters.Models;

namespace VendingMachine.InterfaceAdapters.TestApis
{
    public class ProductTestApi
    {
        private readonly ProductService _service;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<ILogger<ProductService>> _logger;
        private readonly IMapper _mapper;

        public ProductTestApi()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _logger = new Mock<ILogger<ProductService>>();
            _service = new ProductService(_productRepoMock.Object,_logger.Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductInputModel, CreateProductDto>().ReverseMap();
                cfg.CreateMap<ProductInputModel, UpdateProductDto>().ReverseMap();
                cfg.CreateMap<ProductDto, ProductOutputModel>();
                cfg.CreateMap<Product, ProductOutputModel>();
                cfg.CreateMap<Product, ProductInputModel>().ReverseMap();
            });

            _mapper = config.CreateMapper();
        }

        // ---------- Setup Methods ----------

        public void SetupGetAll(List<Product> products)
        {
            _productRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
        }

        public void SetupGetById(int id, Product product)
        {
            _productRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(product);
        }

        public void SetupAdd(ProductInputModel productmodel)
        {
            var product = _mapper.Map<Product>(productmodel);
            _productRepoMock.Setup(r => r.AddAsync(product)).Returns(Task.FromResult(product));
        }

        public void SetupUpdate()
        {
            _productRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
        }

        public void SetupDelete()
        {
            _productRepoMock.Setup(r => r.DeleteAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
        }

        // ---------- API Business Methods ----------

        public async Task<List<ProductOutputModel>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return result.Select(p => _mapper.Map<ProductOutputModel>(p)).ToList();
        }

        public async Task<ProductOutputModel> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return _mapper.Map<ProductOutputModel>(result);
        }

        public async Task AddAsync(ProductInputModel input,string sellerId)
        {
            var dto = _mapper.Map<CreateProductDto>(input);
            await _service.CreateAsync(dto,sellerId);
        }

        public async Task UpdateAsync(int id,ProductInputModel input, string sellerId)
        {
            var dto = _mapper.Map<UpdateProductDto>(input);
            await _service.UpdateAsync(id,dto,sellerId);
        }

        public async Task DeleteAsync(int id, string sellerId)
        {
            await _service.DeleteAsync(id,sellerId);
        }
    }
}
