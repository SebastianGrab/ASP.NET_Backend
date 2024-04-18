using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Data;
using Interfaces;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHostedService<ProtocolCleanupService>();

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Added configuration for PostgreSQL
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
    app.UseExceptionHandler("/Error");
    app.UseHsts();
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
