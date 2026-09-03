using CleanArchitectureDemo.Application.BonusCalculators;
using CleanArchitectureDemo.Application.Commands;
using CleanArchitectureDemo.Application.Interfaces;
using CleanArchitectureDemo.Application.Queries;
using CleanArchitectureDemo.Application.Services;
using CleanArchitectureDemo.Infrastructure.Data;
using CleanArchitectureDemo.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

builder.Services.AddScoped<IEmployeeRepository,
                           EmployeeRepository>();

builder.Services.AddScoped<EmployeeService>();

builder.Services.AddScoped<IBonusCalculator, PermanentBonusCalculator>();
builder.Services.AddScoped<IBonusCalculator, ContractBonusCalculator>();
builder.Services.AddScoped<IBonusCalculator, InternBonusCalculator>();
builder.Services.AddScoped<IBonusCalculator, FreelancerBonusCalculator>();
builder.Services.AddScoped<GetAllEmployeesQueryHandler>();

builder.Services.AddScoped<BonusService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

