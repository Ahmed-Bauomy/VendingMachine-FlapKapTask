using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.Helpers;
using VendingMachine.Application.Interfaces;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Infrastructure.Data;
using VendingMachine.Infrastructure.Repositories;
using VendingMachine.Infrastructure.Services;

namespace VendingMachine.Infrastructure
{
    public static class InfrastructureLayerRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<SeedData>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtTokenGenerator,JwtTokenGenerator>();
            return services;
        }
    }
}
