using apigateway.application.Service;
using apigateway.infrastructure.Auth;
using apigateway.infrastructure.Cache;
using apigateway.infrastructure.EventBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Values;
using RabbitMQ.Client;
using StackExchange.Redis;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));

builder.Services.AddSingleton<RedisCacheService>();
builder.Services.AddScoped<JwtAuthenticationService>();
builder.Services.AddScoped<ModuleAccessService>();

// RabbitMQ Listener as hosted service
builder.Services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory() { HostName = "localhost" });
builder.Services.AddHostedService<RabbitMQListener>();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourIssuer",
            ValidAudience = "yourAudience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"))
        };
    });

// Add Ocelot
builder.Services.AddOcelot();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
