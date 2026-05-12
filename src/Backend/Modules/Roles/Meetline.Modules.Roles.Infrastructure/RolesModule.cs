using Meetline.Modules.Roles.Infrastructure.Data;
using Microsoft.Extensions.Hosting;

namespace Meetline.Modules.Roles.Infrastructure;

public static class RolesModule
{
    extension<TBuilder>(TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        public TBuilder AddRolesModule(Action<RolesModuleOptions> configure)
        {
            var options = new RolesModuleOptions();
            configure(options);

            builder.AddNpgsqlDbContext<RolesDbContext>("postgres-roles");

            return builder;
        }
    }
}