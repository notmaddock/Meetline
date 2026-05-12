using Clerk.BackendAPI;
using Meetline.Modules.Users.Application.Data;
using Meetline.Modules.Users.Application.Repositories;
using Meetline.Modules.Users.Application.Services;
using Meetline.Modules.Users.Infrastructure.Data;
using Meetline.Modules.Users.Infrastructure.Repositories;
using Meetline.Modules.Users.Infrastructure.Services.IdentityProviderClientService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Meetline.Modules.Users.Infrastructure;

public static class UsersModule
{
    extension<TBuilder>(TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        public TBuilder AddUsersModule(Action<UsersModuleOptions> configure)
        {
            var options = new UsersModuleOptions();
            configure(options);

            builder.AddNpgsqlDbContext<UsersDbContext>("postgres-users");

            builder.Services.AddScoped<IUsersDbContext>(sp => sp.GetRequiredService<UsersDbContext>());

            builder.AddServices();

            return builder;
        }

        private TBuilder AddServices()
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IIdentityProviderClientService, ClerkIdentityProviderClientService>();

            var clerkApiKey = builder.Configuration["Clerk:ApiKey"];
            builder.Services.AddSingleton(new ClerkBackendApi(bearerAuth: clerkApiKey));

            return builder;
        }
    }
}