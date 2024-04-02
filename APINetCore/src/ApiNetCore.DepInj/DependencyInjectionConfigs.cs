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


namespace ApiNetCore.DependencyInjection;

public static class DependencyInjectionConfigs
{
    public static IServiceCollection ConfigureDatabaseContext(this IServiceCollection services,
                                                                IConfiguration config)
    {

        var conString = config.GetConnectionString("SqlServerConnectionString");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(conString);
        });
        
        services.AddDbContext<IdentityConfigDbContext>(options =>
        {
            options.UseSqlServer(conString);
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