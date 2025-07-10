using Microsoft.EntityFrameworkCore;
using PathFinder.MigrationService;
using PathFinder.Server.Data;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("postgresdb"),
        x => x.MigrationsAssembly("PathFinder.MigrationService")
    )
);

var host = builder.Build();
host.Run();
