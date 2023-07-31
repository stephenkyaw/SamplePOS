using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService,CustomerService>();
            services.AddScoped<ISaleOrderService, SaleOrderService>();



            services.AddScoped<IPOSService, POSService>();
            services.AddScoped<IPointService, PointService>();

            return services;
        }
    }
}
