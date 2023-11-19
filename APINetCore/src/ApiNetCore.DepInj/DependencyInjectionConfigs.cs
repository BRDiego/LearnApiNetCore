using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Data.EFContext.Repository;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using ApiNetCore.Application.Services;
using ApiNetCore.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ApiNetCore.Data.EFContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
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

        return services;
    }
}