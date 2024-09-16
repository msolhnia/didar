using EventBus.Interface;
using Packaging.application.Contract.api.Interface;
using Packaging.application.Contract.infrastructure;
using Packaging.application.Service;
using Packaging.infrastructure.Data.Persist;
using Packaging.infrastructure.Data.Repository;
using RabbitMQ.Client;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//injections start
// Add DbContext
builder.Services.AddDbContext<PackagingContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();

// Add event bus
builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
builder.Services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory() { HostName = "localhost" });

// Add services
builder.Services.AddScoped<IModuleService, ModuleService>();

//injections end



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
