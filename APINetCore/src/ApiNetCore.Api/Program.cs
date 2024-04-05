using ApiNetCore.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureDatabaseContext(builder.Configuration);
builder.Services.ConfigureDependencyInjection();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("Production");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
