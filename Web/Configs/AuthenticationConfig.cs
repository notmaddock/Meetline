using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Web.Tenancy;

namespace Web.Configs;

public static class AuthenticationConfig
{
    internal const string TenantIdHttpContextKey = "TenantId";

    internal static IServiceCollection AddMultiTenantJwtValidation(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer()
            .AddJwtBearer("TenantManagement",
                options => { builderConfiguration.GetSection("ManagementJwtBearer").Bind(options); });

        services.AddSingleton<OpenIdConfigurationService>();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<OpenIdConfigurationService>((options, configService) =>
            {
                builderConfiguration.GetSection("JwtBearer").Bind(options);

                options.TokenValidationParameters.IssuerValidator = (issuer, token, parameters) =>
                    configService.GetTenantId(issuer) == null
                        ? throw new SecurityTokenInvalidIssuerException($"Unknown tenant issuer: {issuer}")
                        : issuer;

                options.TokenValidationParameters.IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                    configService.GetKeys(securityToken.Issuer);

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async context =>
                    {
                        var token = context.Request.Headers.Authorization.ToString();
                        if (string.IsNullOrEmpty(token) ||
                            !token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            return;

                        var jwt = token["Bearer ".Length..].Trim();
                        var handler = new JsonWebTokenHandler();
                        if (!handler.CanReadToken(jwt)) return;

                        var decoded = handler.ReadJsonWebToken(jwt);
                        var issuer = decoded.Issuer;

                        // Ensure we have the tenant ID for this issuer
                        var tenantId = configService.GetTenantId(issuer);
                        if (tenantId == null)
                            await configService.RefreshTenantMappingsAsync(context.HttpContext.RequestAborted);

                        // Pre-fetch keys if we found a tenant
                        if (configService.GetTenantId(issuer) != null)
                            await configService.PreFetchConfigurationAsync(issuer, context.HttpContext.RequestAborted);
                    },
                    OnTokenValidated = context =>
                    {
                        var issuer = context.SecurityToken.Issuer;
                        var tenantId = configService.GetTenantId(issuer);

                        if (tenantId.HasValue) context.HttpContext.Items[TenantIdHttpContextKey] = tenantId.Value;

                        return Task.CompletedTask;
                    }
                };
            });


        return services;
    }
}