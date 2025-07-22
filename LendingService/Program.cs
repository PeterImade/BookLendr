using BookService.Application.Extensions;
using Contracts.Events;
using Contracts.Extensions;
using LendingService.Application.Interfaces;
using LendingService.Infrastructure.Data;
using LendingService.Infrastructure.Repositories;
using LendingService.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalErrorMiddleware>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<ILendingRepository, LendingRepository>();
builder.Services.AddScoped<ILendingService, LendingBLService>();
builder.Services.AddHttpClient<BookServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5115");
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });

    x.AddRequestClient<CheckBookAvailabilityRequest>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
