using System.Text.Json;
using FluentValidation;
using Infrastructure;
using Mediator;
using Meetline.Modules.SharedKernel.Application.CQRS.Caching;
using Meetline.Modules.SharedKernel.Application.CQRS.PipelineBehaviors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
// TODO re-enable caching
// builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddValidatorsFromAssembly(typeof(ICacheService).Assembly);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { builder.Configuration.GetSection("JwtBearer").Bind(options); });

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

root.MapEndpoints();

app.Run();