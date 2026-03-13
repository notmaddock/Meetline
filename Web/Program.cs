using Infrastructure;
using Scalar.AspNetCore;
using Web.Configs;
using Web.Endpoints;
using Web.Endpoints.V1;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure()
    .AddOpenApi(OpenApiConfiguration.Configure)
    .AddApiVersioning(ApiVersioningConfiguration.Configure);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    app.MapGroup("/_debug").MapDebugV1Endpoints();
}

app.MapEndpoints();

app.Run();