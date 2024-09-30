using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using JobBoardApp.Domain.Entities;
using JobBoardApp.Infrastructure.Data;

namespace JobBoardApp.Infrastructure.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            // adding identity
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
