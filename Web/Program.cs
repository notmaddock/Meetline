using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Scalar.AspNetCore;
using Web.Configs;
using Web.Endpoints;
using Web.Endpoints.V1;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { builder.Configuration.GetSection("JwtBearer").Bind(options); });

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

builder.Services
    .AddInfrastructure()
    .AddOpenApi(OpenApiConfiguration.Configure)
    .AddApiVersioning(ApiVersioningConfiguration.Configure);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    app.MapGroup("/_debug").AllowAnonymous().MapDebugV1Endpoints();
}

app.MapGet("/public", () => "welcome to a public endpoint").AllowAnonymous();
app.MapGet("/private", () => "welcome to a private endpoint!");

app.MapEndpoints();

app.Run();