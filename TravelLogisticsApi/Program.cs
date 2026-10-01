using Scalar.AspNetCore;
using TravelLogisticsApi.Data;
using TravelLogisticsApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173",
                                    "http://localhost:3000",
                                    "https://travel-logistics-api-gzh7chc3gvg4eye8.northcentralus-01.azurewebsites.net")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IBorderRepository, BorderRepository>();
builder.Services.AddTransient<IRoutingService, RoutingService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(); 
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
