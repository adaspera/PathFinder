using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using PathFinder.MigrationService;
using PathFinder.Server.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<ApiDbInitializer>();

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresdb"), npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly("PathFinder.MigrationService");
    }));
builder.EnrichNpgsqlDbContext<AppDbContext>(settings =>
    settings.DisableRetry = true);

Console.WriteLine("🔐 Connection: " + builder.Configuration.GetConnectionString("postgresdb"));

var app = builder.Build();
app.Run();
