using ApiNetCore.DependencyInjection;
using APINetCore.Api.Configuration;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureDependencyInjection();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfiguration>();

builder.Services.ConfigureApi();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerConfig();

var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
    app.UseSwaggerConfig(apiVersionDescriptionProvider);
}
else
{
    app.UseCors("Production");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
