using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VendingMachine.Application.Interfaces;
using VendingMachine.Application.Services;

namespace VendingMachine.Application
{
    public static class ApplicationLayerRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPurchaseService, PurchaseService>();

            services.Decorate<IProductService, ProductServiceLoggingDecorator>();
            services.Decorate<IPurchaseService, PurchaseServiceLoggingDecorator>();

            return services;
        }
    }
}
