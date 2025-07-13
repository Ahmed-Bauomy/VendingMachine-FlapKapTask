using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.Settings;
using VendingMachine.Application.DTOs.User;
using VendingMachine.Application.Helpers;
using VendingMachine.Application.Interfaces;
using VendingMachine.Infrastructure.Data;
using VendingMachine.Infrastructure.Services;
using VendingMachine.InterfaceAdapters.Models;

namespace VendingMachine.InterfaceAdapters.TestApis
{
    public class UserTestApi
    {
        private readonly UserService _service;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenMock;
        private readonly IMapper _mapper;

        public UserTestApi()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(_userManagerMock.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

            var jwtSettings = new JwtSettings
            {
                Key = "Better_have_it_somewhere_safer!!",
                ValidIssuer = "localhost",
                ValidAudience = "localhost",
                TokenExpiresInMinutes = 10
            };

            var optionsMock = Options.Create(jwtSettings);
            _jwtTokenMock = new Mock<IJwtTokenGenerator>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserInputModel, RegisterUserDto>();
                cfg.CreateMap<RegisterUserDto, UserOutputModel>();
                cfg.CreateMap<UserDto, UserOutputModel>().ForMember(u => u.Role,config => config.MapFrom(u => u.Roles.FirstOrDefault()));
                cfg.CreateMap<UserOutputModel, UserDto>();
                cfg.CreateMap<ApplicationUser, UserDto>().ReverseMap();
            });

            _mapper = config.CreateMapper();

            _service = new UserService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _roleManagerMock.Object,
                _jwtTokenMock.Object
            );
        }

        // === SETUPS ===

        public void SetupRegister(UserInputModel input, UserOutputModel expected,List<string> roles)
        {
            var user = new UserDto { Id = expected.Id, UserName = expected.Username };
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), input.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), input.Role))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.FindByNameAsync(input.Username)).ReturnsAsync(_mapper.Map<ApplicationUser>(user));
            _roleManagerMock.Setup(r => r.RoleExistsAsync("Buyer")).Returns(Task.FromResult(true));
            _roleManagerMock.Setup(r => r.RoleExistsAsync("Seller")).Returns(Task.FromResult(true));
            _jwtTokenMock.Setup(j => j.GenerateToken(user,roles)).Returns(expected.Token);
        }

        public void SetupLogin(LoginInputModel input, UserOutputModel expected, List<string> roles)
        {
            var user = new UserDto { Id = expected.Id, UserName = expected.Username };
            var appUser = _mapper.Map<ApplicationUser>(user);
            _userManagerMock.Setup(m => m.FindByNameAsync(input.Username)).ReturnsAsync(appUser);
            _signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(appUser, input.Password, false))
                .ReturnsAsync(SignInResult.Success);
            _userManagerMock.Setup(m => m.GetRolesAsync(appUser)).ReturnsAsync(roles);
            _jwtTokenMock.Setup(j => j.GenerateToken(It.Is<UserDto>(u => u.Id == expected.Id),
                                                     (IList<string>)It.Is<IEnumerable<string>>(r => r.SequenceEqual(roles))))
                         .Returns(expected.Token);
        }

        public void SetupGetAll(List<UserOutputModel> expectedList)
        {
            var dtos = expectedList.Select(_mapper.Map<UserDto>).ToList();
            _userManagerMock.Setup(m => m.Users).Returns(dtos.Select(d => new ApplicationUser { Id = d.Id, UserName = d.UserName }).AsQueryable());
        }

        public void SetupGetById(string id, UserOutputModel expected)
        {
            var user = new ApplicationUser { Id = expected.Id, UserName = expected.Username };
            _userManagerMock.Setup(m => m.FindByIdAsync(id)).ReturnsAsync(user);
        }

        public void SetupDelete(string id, bool result)
        {
            var user = new ApplicationUser { Id = id };
            _userManagerMock.Setup(m => m.FindByIdAsync(id)).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(result ? IdentityResult.Success : IdentityResult.Failed());
        }

        public void SetupUpdate(string id, UserInputModel input, bool result)
        {
            var user = new ApplicationUser { Id = id };
            _userManagerMock.Setup(m => m.FindByIdAsync(id)).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(result ? IdentityResult.Success : IdentityResult.Failed());
        }

        public void SetupUpdateDeposit(string userId)
        {
            var user = new ApplicationUser { Id = userId };
            _userManagerMock.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        }

        // === CALL METHODS ===

        public async Task<UserOutputModel> RegisterAsync(UserInputModel input)
        {
            var dto = _mapper.Map<RegisterUserDto>(input);
            var result = await _service.RegisterAsync(dto);
            return _mapper.Map<UserOutputModel>(result);
        }

        public async Task<UserOutputModel> LoginAsync(LoginInputModel input)
        {
            var dto = new LoginUserDto { Username = input.Username, Password = input.Password };
            var result = await _service.LoginAsync(dto);
            return _mapper.Map<UserOutputModel>(result);
        }

        public async Task<IEnumerable<UserOutputModel>> GetAllAsync()
        {
            var users = await _service.GetAllAsync();
            return users.Select(_mapper.Map<UserOutputModel>);
        }

        public async Task<UserOutputModel?> GetByIdAsync(string id)
        {
            var user = await _service.GetByIdAsync(id);
            return _mapper.Map<UserOutputModel>(user);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _service.DeleteAsync(id);
        }

        public async Task<bool> UpdateAsync(string id, UserInputModel input)
        {
            var dto = _mapper.Map<RegisterUserDto>(input);
            return await _service.UpdateAsync(id, dto);
        }

        public async Task UpdateDepositAsync(string userId, int deposit)
        {
            await _service.UpdateDepositAsync(userId, deposit);
        }
    }
}
