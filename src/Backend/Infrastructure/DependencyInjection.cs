using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddMemoryCache();
        // services.AddScoped<ICacheService, MemoryCacheService>();
        //
        // services.AddDbContext<ApplicationDbContext>(options =>
        // {
        //     options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        // });
        //
        // services.AddSingleton(new ClerkBackendApi(configuration["Clerk:SecretKey"]));
        //
        // services.AddScoped<IIdentityProviderClientService, ClerkIdentityProviderClientService>();
        //
        // services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }
}