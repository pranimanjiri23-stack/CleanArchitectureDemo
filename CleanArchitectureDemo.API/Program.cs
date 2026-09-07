using CleanArchitectureDemo.Application.BonusCalculators;
using CleanArchitectureDemo.Application.Configuration;
using CleanArchitectureDemo.Application.Interfaces;
using CleanArchitectureDemo.Application.Queries;
using CleanArchitectureDemo.Application.Services;
using CleanArchitectureDemo.Infrastructure.Authentication;
using CleanArchitectureDemo.Infrastructure.Data;
using CleanArchitectureDemo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<GetAllEmployeesQueryHandler>();
builder.Services.AddScoped<GetEmployeeByIdQueryHandler>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<BonusService>();


//// JWT Authentication
//var jwtKey = builder.Configuration["Jwt:Key"]
//    ?? throw new InvalidOperationException("JWT Key is not configured.");

//var jwtIssuer = builder.Configuration["Jwt:Issuer"]
//    ?? throw new InvalidOperationException("JWT Issuer is not configured.");

//var jwtAudience = builder.Configuration["Jwt:Audience"]
//    ?? throw new InvalidOperationException("JWT Audience is not configured.");

var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException("JWT configuration is not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.Key))
            };
    });

builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

