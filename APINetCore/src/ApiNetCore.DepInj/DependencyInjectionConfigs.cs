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
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace ApiNetCore.DependencyInjection;

public static class DependencyInjectionConfigs
{
    public static void ConfigureDatabase(ref WebApplicationBuilder builder)
    {
        var conString = builder.Configuration.GetConnectionString("SqlServerConnectionString");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(conString);
        });
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