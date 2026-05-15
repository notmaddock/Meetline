using Meetline.Modules.Roles.Infrastructure.Data;
using Meetline.Modules.SharedKernel.Infrastructure.Initialization;
using Meetline.ServiceDefaults;
using Meetline.ServiceDefaults.Initialization;

var builder = Host.CreateApplicationBuilder(args);

builder.AddNpgsqlDbContext<RolesDbContext>("postgres-roles");
builder.Services.AddScoped<IDataInitializer, EfCoreInitializer<RolesDbContext>>();

builder.AddMigrationService();

var host = builder.Build();
host.Run();