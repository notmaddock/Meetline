using Application.Common.Caching;
using Application.InternalServices;
using Application.Repositories;
using Infrastructure.Common.Caching;
using Infrastructure.Data;
using Infrastructure.InternalServices;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();

        services.AddSingleton<ITenantReadService, TenantReadService>();

        services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }
}