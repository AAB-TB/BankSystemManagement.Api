using BankSystemManagement.Core.AutoMapper;
using BankSystemManagement.Core.Interfaces;
using BankSystemManagement.Core.Services;
using BankSystemManagement.Data.DataModels;
using BankSystemManagement.Data.Interfaces;
using BankSystemManagement.Data.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

// Get JWT secret key from configuration
string jwtSecretKey = builder.Configuration.GetSection("Jwt:SecretKey").Value;

// Configure and add JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    ConfigureJwtBearerOptions(options, jwtSecretKey);
});

// Add DapperContext and other services
AddCustomServices(builder.Services);

// Configure Swagger
ConfigureSwagger(builder.Services);

// Configure Serilog for logging
ConfigureSerilog();

var app = builder.Build();

// Get the hosting environment
var env = app.Services.GetRequiredService<IHostEnvironment>();

// Configure the HTTP request pipeline
ConfigureMiddlewarePipeline(app, env);

app.Run();

// Helper methods

void ConfigureJwtBearerOptions(JwtBearerOptions options, string secretKey)
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = "https://localhost:44362",
        ValidateIssuer = true,
        ValidIssuer = "https://localhost:44362",
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.FromMinutes(1),
    };
}

void AddCustomServices(IServiceCollection services)
{
    services.AddScoped<DapperContext>();
    services.AddAutoMapper(typeof(MappingProfile));

    services.AddScoped<IUserService,UserService>();
    services.AddScoped<IUserRepo, UserRepo>();

    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<IAccountRepo, AccountRepo>();

    services.AddScoped<ILoanService, LoanService>();
    services.AddScoped<ILoanRepo, LoanRepo>();

    services.AddControllers();

}

void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Bank System Management API",
            Version = "v1",
            Description = "API for managing banking operations.",
            Contact = new OpenApiContact
            {
                Name = "Alvin Alamgir Berglund",
                Email = "alvinalamgirberglund@gmail.com",
                Url = new Uri("https://www.AABTB.com")
            }
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new List<string>()
            }
        });
    });
}

void ConfigureSerilog()
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("log.txt")
        .CreateLogger();

    builder.Host.UseSerilog();
}



void ConfigureMiddlewarePipeline(WebApplication app, IHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bank System Management API V1");
            // Add more UI customization options if needed
        });
    }
    else
    {
        app.UseExceptionHandler("/error");
        app.UseHttpsRedirection();
    }

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<RoleAuthorizationMiddleware>();
    app.MapControllers();
}
