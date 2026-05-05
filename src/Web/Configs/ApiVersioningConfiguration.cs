using Asp.Versioning;

namespace Web.Configs;

public static class ApiVersioningConfiguration
{
    public static void Configure(ApiVersioningOptions options)
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;

        options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
    }
}