using BattleShip.API.Constraints;
using BattleShip.API.Endpoints.Field;
using BattleShip.API.Endpoints.Session;
using BattleShip.Application;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.Configure<RouteOptions>(options =>
 {
     options.ConstraintMap.Add("string", typeof(StringConstraint));
 });

 builder.Services.AddBattleFieldApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.RegisterFieldEndpoints();
app.RegisterSessionEndpoints();

app.MapGet("/ping", () =>
{    
    return "pong";
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
