using ApiNetCore.Business.AlertsManagement;
using ApiNetCore.Business.Services;
using ApiNetCore.Business.Services.Interfaces;
using ApiNetCore.Data.EFContext.Repository;
using ApiNetCore.Data.EFContext.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace ApiNetCore.DependencyInjection;

public class DependencyInjectionConfigs 
{
    public static void Configure(ref WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IBandRepository, BandRepository>();
        services.AddScoped<IMusicianRepository, MusicianRepository>();

        services.AddScoped<IAlertManager, AlertManager>();
        
        services.AddScoped<IBandService, BandService>();
        services.AddScoped<IMusicianService, MusicianService>();
    }
}