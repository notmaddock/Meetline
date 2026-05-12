using Meetline.Modules.SharedKernel.Infrastructure.Initialization;
using Meetline.Modules.Users.Infrastructure.Data;
using Meetline.ServiceDefaults;
using Meetline.ServiceDefaults.Initialization;

var builder = Host.CreateApplicationBuilder(args);

builder.AddNpgsqlDbContext<UsersDbContext>("postgres-users");
builder.Services.AddScoped<IDataInitializer, EfCoreInitializer<UsersDbContext>>();

builder.AddMigrationService();

var host = builder.Build();
host.Run();