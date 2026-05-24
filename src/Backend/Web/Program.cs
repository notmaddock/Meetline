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

builder.Host.UseWolverine(options =>
{
    options.Discovery.IncludeAssembly(typeof(AssemblyReference).Assembly);
    options.Discovery.IncludeAssembly(typeof(Meetline.Modules.Users.Application.AssemblyReference).Assembly);

    options.Policies.AddMiddleware<ClaimsPrincipalCallerContextProviderMiddleware>(chain =>
        chain.Handlers.Any(h => h.Method.GetParameters().Any(p => p.ParameterType == typeof(ICallerContext))));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            {
                if (Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                    return uri.Host == "localhost" || uri.Host == "127.0.0.1" || uri.Host.EndsWith(".maddock.world");

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
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/api/gateway"))
                    context.Token = accessToken;

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