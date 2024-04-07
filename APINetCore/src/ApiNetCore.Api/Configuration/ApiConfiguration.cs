namespace APINetCore.Api.Configuration
{
    public static class ApiConfiguration
    {
        public static IServiceCollection ConfigureApi(this IServiceCollection services)
        {

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(2, 0);
                options.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            //Cors is not implemented for lots of apps. Browsers do. Apps or services like postman may not do so.
            services.AddCors(options =>
            {
                options.AddPolicy("Development", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });

                options.AddPolicy("Production", builder =>
                {
                    builder
                    .WithMethods("GET", "PUT")
                    .WithOrigins("https://mywebsite.domain", "https://another.domain")
                    .AllowCredentials() //this one is not that safe, since it's easy to simulate credentials
                    .SetIsOriginAllowedToAllowWildcardSubdomains() //allow origins subdomains
                                                                   //.WithHeaders(HeaderNames.ContentType, "application/json") restricting the ones above is good enough
                    .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
