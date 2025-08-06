using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Data;
using FluentValidation;
using UserService.Infrastructure.Repositories;
using UserService.Exceptions;
using System.Reflection;
using UserService.Application.Queries;
using UserService.Application.Validators;
using UserService.Application.DTOs;
using FluentValidation.AspNetCore;
using MassTransit;
using UserService.Infrastructure.Services;
using UserService.Application.Interfaces;
using Hangfire;
using UserService.Infrastructure.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserBLService>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,
        typeof(GetUserByEmailQuery).Assembly
    );
});

builder.Services.AddMassTransit(x => x.UsingRabbitMq((context, cfg) =>
{
    cfg.Host("localhost", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });
}));

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHangfireServer();



builder.Services.AddExceptionHandler<GlobalMiddlewareHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<OutboxPublisherService>("publish-outbox-events", service => service.PublishUnsentEventsAsync(), Cron.Minutely);

app.UseAuthorization();

app.MapControllers();

app.Run();
