using System.Text.Json;
using Meetline.Modules.Roles.Infrastructure;
using Meetline.Modules.SharedKernel.Application.Context;
using Meetline.Modules.Users.Infrastructure;
using Meetline.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.SignalR;
using Scalar.AspNetCore;
using Web.Configs;
using Web.Converters;
using Web.Endpoints;
using Web.Endpoints.V1;
using Web.Hubs;
using Web.Hubs.Filters;
using Web.Middlewares;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            {
                if (Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                {
                    var isLocal = uri.Host == "localhost" || uri.Host == "127.0.0.1";
                    if (isLocal) return builder.Environment.IsDevelopment();

                    return uri.Host == "maddock.world" || uri.Host.EndsWith(".maddock.world");
                }

                return false;
            })
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSignalR(options => { options.AddFilter<IdentityResolutionFilter>(); });

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        builder.Configuration.GetSection("JwtBearer").Bind(options);

        options.Events = new JwtBearerEvents
        {
            // Read access token from ?access_token query param if going to gateway since WS doesn't support headers
            OnMessageReceived = context =>
            {
                var path = context.HttpContext.Request.Path;
                if (path.StartsWithSegments("/api/gateway"))
                {
                    var accessToken = context.Request.Query["access_token"];
                    if (!string.IsNullOrEmpty(accessToken)) context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

builder.Services
    .AddOpenApi(OpenApiConfiguration.Configure)
    .AddApiVersioning(ApiVersioningConfiguration.Configure);

builder.Services.AddProblemDetails();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.Converters.Add(new PermissionSetJsonConverter());
});

builder.Services.AddExceptionHandler<BadHttpRequestExceptionHandler>();

builder.AddUsersModule(_ => { });
builder.AddRolesModule(_ => { });

builder.Host.UseWolverine(options =>
{
    options.Discovery.IncludeAssembly(typeof(AssemblyReference).Assembly);
    options.Discovery.IncludeAssembly(typeof(Meetline.Modules.Users.Application.AssemblyReference).Assembly);

    var opaqueInterfaces = builder.Services
        .Where(descriptor =>
            descriptor.ServiceType.IsInterface &&
            descriptor.ImplementationFactory != null)
        .Select(descriptor => descriptor.ServiceType)
        .Distinct()
        .ToList();

    foreach (var serviceType in opaqueInterfaces) options.CodeGeneration.AlwaysUseServiceLocationFor(serviceType);

    options.Policies.AddMiddleware<ClaimsPrincipalCallerContextProviderMiddleware>(chain =>
        chain.Handlers.Any(h => h.Method.GetParameters().Any(p => p.ParameterType == typeof(ICallerContext))));
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/api/openapi").AllowAnonymous();
    app.MapScalarApiReference("/api/scalar", options => { options.OpenApiRoutePattern = "/api/openapi"; })
        .AllowAnonymous();

    app.MapGroup("/api/_debug").AllowAnonymous().MapDebugV1Endpoints();
}

var root = app.MapGroup("");

root.MapEndpoints();

app.MapHub<GatewayHub>("/api/gateway");

app.Run();