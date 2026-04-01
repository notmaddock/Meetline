using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            .AddJwtBearer();

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