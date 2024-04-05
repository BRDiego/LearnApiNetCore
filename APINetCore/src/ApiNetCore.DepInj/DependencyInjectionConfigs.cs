using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Data.EFContext.Repository;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Application.Services;
using ApiNetCore.Application.Services.Interfaces;
using ApiNetCore.Application.DTOs.Interfaces;
using ApiNetCore.Data.EFContext;
using ApiNetCore.Application.DTOs.Validations.BusinessRulesValidators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ApiNetCore.Application.DTOs.Authentication;
using Microsoft.AspNetCore.Http;
using ApiNetCore.Business.Interfaces;
using ApiNetCore.Application.Extensions;
using Microsoft.Net.Http.Headers;

namespace ApiNetCore.DependencyInjection;

public static class DependencyInjectionConfigs
{
    public static IServiceCollection ConfigureDatabaseContext(this IServiceCollection services,
                                                                IConfiguration configuration)
    {

        var conString = configuration.GetConnectionString("SqlServerConnectionString");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(conString);
        });

        services.AddDbContext<IdentityConfigDbContext>(options =>
        {
            options.UseSqlServer(conString);
        });

        services.AddIdentity<IdentityUser, IdentityRole>().
            AddEntityFrameworkStores<IdentityConfigDbContext>().
            AddDefaultTokenProviders();

        //JWT

        var appHandshakeSettingsSection = configuration.GetSection("AppHandshakeSettings");
        services.Configure<AppHandshakeSettings>(appHandshakeSettingsSection);

        var appHandshakeSettings = appHandshakeSettingsSection.Get<AppHandshakeSettings>();
        var key = Encoding.ASCII.GetBytes(appHandshakeSettings!.Secret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearer =>
        {
            bearer.RequireHttpsMetadata = true;
            bearer.SaveToken = true;
            bearer.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = appHandshakeSettings.Sender,
                ValidAudience = appHandshakeSettings.ValidSpectator
            };
        });

        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IBandRepository, BandRepository>();
        services.AddScoped<IMusicianRepository, MusicianRepository>();

        services.AddScoped<IAlertManager, AlertManager>();

        services.AddScoped<IBandService, BandService>();
        services.AddScoped<IMusicianService, MusicianService>();

        services.AddScoped<IBusinessRules, BusinessRulesValidator>();

        services.AddScoped<IUser, ApiUser>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        return services;
    }

    public static IServiceCollection ConfigureApi(this IServiceCollection services)
    {

        //Cors is not implemented for lots of apps. Browsers do. Apps or services like postman may not do so.
        services.AddCors(options =>
        {
            options.AddPolicy("Development", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); //this one is not that safe, since it's easy to simulate credentials
            });

            options.AddPolicy("Production", builder =>
            {
                builder
                .WithMethods("GET", "PUT")
                .WithOrigins("https://mywebsite.domain", "https://another.domain")
                .SetIsOriginAllowedToAllowWildcardSubdomains() //allow origins subdomains
                //.WithHeaders(HeaderNames.ContentType, "application/json") restricting the ones above is good enough
                .AllowAnyHeader();
            });
        });

        return services;
    }
}