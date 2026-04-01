using System.Text.Json;
using Application.Common.Caching;
using Application.Common.PipelineBehaviors;
using FluentValidation;
using Infrastructure;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Scalar.AspNetCore;
using Web.Configs;
using Web.Converters;
using Web.Endpoints;
using Web.Endpoints.V1;
using Web.Filters;
using Web.Scopes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediator(options => { options.ServiceLifetime = ServiceLifetime.Scoped; });
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddValidatorsFromAssembly(typeof(ICacheService).Assembly);


builder.Services.AddMultiTenantJwtValidation(builder.Configuration);

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddOpenApi(OpenApiConfiguration.Configure)
    .AddApiVersioning(ApiVersioningConfiguration.Configure);

builder.Services.AddProblemDetails();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.Converters.Add(new PermissionSetJsonConverter());
});

builder.Services.AddScoped<CurrentUserScope>();

builder.Services.AddExceptionHandler<BadHttpRequestExceptionHandler>();

var app = builder.Build();

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

var root = app.MapGroup("").AddEndpointFilter<UserScopeInitializationFilter>();

root.MapGet("api/_debug/authcheck", (CurrentUserScope scope) => Results.Ok((object?)scope));

root.MapEndpoints();

app.Run();