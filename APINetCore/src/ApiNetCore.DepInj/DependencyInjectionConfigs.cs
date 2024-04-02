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
using ApiNetCore.Application.DTOs.Extensions;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

        services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        return services;
    }
}