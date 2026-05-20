using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var clerkApiKey = builder.AddParameterFromConfiguration("clerk-api-key", "Clerk:ApiKey", true);
var clerkWebhookSecret =
    builder.AddParameterFromConfiguration("clerk-webhook-secret", "Clerk:WebhookSecret", true);
var clerkPublishableKey = builder.AddParameterFromConfiguration("clerk-publishable-key", "Clerk:PublishableKey");

var redis = builder.AddRedis("redis");

var postgres = builder.AddPostgres("postgres-master")
    .WithDataVolume();

var usersPostgres = postgres.AddDatabase("postgres-users");
var rolesPostgres = postgres.AddDatabase("postgres-roles");

var usersMigrationService = builder.AddProject<Meetline_Modules_Users_MigrationService>("user-service-migrations")
    .WithReference(usersPostgres)
    .WaitFor(usersPostgres);

var rolesMigrationService = builder.AddProject<Meetline_Modules_Roles_MigrationService>("role-service-migrations")
    .WithReference(rolesPostgres)
    .WaitFor(rolesPostgres);


var backend = builder.AddProject<Web>("meetline-backend")
    .WithReference(redis)
    .WithReference(usersPostgres)
    .WithReference(rolesPostgres)
    .WithReference(usersMigrationService)
    .WaitForCompletion(usersMigrationService)
    .WithReference(rolesMigrationService)
    .WaitForCompletion(rolesMigrationService)
    .WithEnvironment("Clerk__ApiKey", clerkApiKey)
    .WithEnvironment("Clerk__WebhookSecret", clerkWebhookSecret)
    .WaitFor(postgres);

var frontend = builder.AddViteApp("frontend", "../../Frontend/MeetlineUI")
    .WithPnpm()
    .WithEnvironment("VITE_CLERK_PUBLISHABLE_KEY", clerkPublishableKey)
    .WithEnvironment("VITE_API_BASE_URL", backend.GetEndpoint("https"))
    .WithReference(backend)
    .WaitFor(backend);

builder.Build().Run();