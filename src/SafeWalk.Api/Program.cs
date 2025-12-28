using Microsoft.Extensions.Configuration;
using SafeWalk.Application.Interfaces.Services;
using SafeWalk.Application.Services;
using SafeWalk.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infrastructure (SQL, repos, time, SMS)
builder.Services.AddInfrastructure(builder.Configuration);

// Application services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJourneyService, JourneyService>();
builder.Services.AddScoped<IAlertService, AlertService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
