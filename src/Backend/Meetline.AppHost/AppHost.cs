var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres-master")
    .WithDataVolume();

var usersPostgres = postgres.AddDatabase("postgres-users");
var rolesPostgres = postgres.AddDatabase("postgres-roles");

builder.AddProject<Projects.Web>("meetline-backend")
    .WithReference(usersPostgres)
    .WithReference(rolesPostgres)
    .WaitFor(postgres);

builder.Build().Run();