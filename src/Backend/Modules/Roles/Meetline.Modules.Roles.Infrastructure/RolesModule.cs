using Meetline.Modules.Roles.Application.Data;
using Meetline.Modules.Users.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Meetline.Modules.Roles.Infrastructure;

public static class RolesModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRolesModule(Action<RolesModuleOptions> configure)
        {
            var options = new RolesModuleOptions();

            configure(options);

            services.AddDbContext<RolesDbContext>(db => { db.UseNpgsql(options.ConnectionString); });

            return services;
        }
    }
}