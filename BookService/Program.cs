using BookService.Application.Commands;
using BookService.Application.Extensions;
using BookService.Application.Interfaces;
using BookService.Application.Validators;
using BookService.Infrastructure.Data;
using BookService.Infrastructure.Repositories;
using BookService.Infrastructure.Services;
using Contracts.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
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
        typeof(CreateBookCommand).Assembly
    );
});

builder.Services.AddValidatorsFromAssemblyContaining(typeof(BookValidator));
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
