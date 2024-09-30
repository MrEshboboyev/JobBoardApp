using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.Infrastructure.Data;
using JobBoardApp.Infrastructure.Implementations;
using JobBoardApp.Infrastructure.Repositories;

namespace JobBoardApp.Infrastructure.Configurations
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // adding lifetimes
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
