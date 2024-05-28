using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Data;
using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models;
using Repository;
using Serilog;
using Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                                "https://localhost:3000",
                                "http://localhost:8080",
                                "https://localhost:8080")
                                .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHostedService<ProtocolCleanupService>();
builder.Services.AddHostedService<PasswordUpdateMessageService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAdditionalUserRepository, AdditionalUserRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IProtocolRepository, ProtocolRepository>();
builder.Services.AddScoped<IProtocolContentRepository, ProtocolContentRepository>();
builder.Services.AddScoped<IProtocolPdfFileRepository, ProtocolPdfFileRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();
builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
builder.Services.AddScoped<ITemplateOrganizationRepository, TemplateOrganizationRepository>();
builder.Services.AddScoped<IUserMessageRepository, UserMessageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserLoginAttemptRepository, UserLoginAttemptRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddAuthentication(options =>
    {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.Requirements.Add(new RoleRequirement("Admin")));
    options.AddPolicy("LeiterPolicy", policy =>
        policy.Requirements.Add(new RoleRequirement("Leiter")));
    options.AddPolicy("HelferPolicy", policy =>
        policy.Requirements.Add(new RoleRequirement("Helfer")));
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a Token.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[]{}
        }
    });
});

builder.Services.AddAuthorization();

var configuration = builder.Configuration;
builder.Services.AddDbContext<ProtocolContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("ConnectionString")));

builder.Host.UseSerilog((context, configuration) =>
{
    var connectionString = context.Configuration.GetConnectionString("ConnectionString");

    configuration.WriteTo.PostgreSQL(connectionString, "Logs", needAutoCreateTable: true)
        .MinimumLevel.Information();
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("RateLimiting", policy =>
    {
        policy.Window = TimeSpan.FromMinutes(1);
        policy.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        policy.QueueLimit = 0;
        policy.PermitLimit = 100;
    });
});

var app = builder.Build();

app.UseRateLimiter();

if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Error");
    app.UseHsts();

    app.Use(async (context, next) =>
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            // Log the exception details
            var logger = context.RequestServices.GetService<ILogger<Program>>();  
            logger.LogError(ex, "An error occurred while processing the request.");

            context.Response.StatusCode = 404;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(
                "Not Found. Error while processing the request.");
        }
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DRK WebApp Endpunkte (.NET Version)");
        c.DocumentTitle = "DRK WebApp";
    });
}

app.UseHttpsRedirection();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ProtocolContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();