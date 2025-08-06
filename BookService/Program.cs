using BookService.Application.Commands;
using BookService.Application.Extensions;
using BookService.Application.Interfaces;
using BookService.Application.Validators;
using BookService.Consumers;
using BookService.Infrastructure.Data;
using BookService.Infrastructure.Repositories;
using BookService.Infrastructure.Services;
using Contracts.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,
        typeof(CreateBookCommand).Assembly //test this
    );
});

builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<CheckBookAvailabilityConsumer>();
    options.AddConsumer<BookReturnedConsumer>();

    options.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        configuration.ReceiveEndpoint("check-book-availability", e =>
        {
            e.ConfigureConsumer<CheckBookAvailabilityConsumer>(context);
        });
        
        configuration.ReceiveEndpoint("book-returned", e =>
        {
            e.ConfigureConsumer<BookReturnedConsumer>(context);
        });
    });
});

builder.Services.AddValidatorsFromAssemblyContaining(typeof(BookValidator));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(BookUpdateValidator));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IBookBLService, BookBLService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddExceptionHandler<GlobalErrorMiddleware>();
builder.Services.AddProblemDetails();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
