using Serilog;
using Aspire.Hosting;
using Serilog.Events;


try
{
    string basePath = AppContext.BaseDirectory;
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File($"{basePath}/Logging/Logs.log", rollingInterval: RollingInterval.Day)
        .CreateLogger();
    
    Log.Information("Starting PSK AppHost");

    var builder = (DistributedApplicationBuilder)DistributedApplication.CreateBuilder(args);
    
    var postgres = builder.AddPostgres("postgres")
        .WithDataVolume()
        .WithPgWeb();

    var postgresdb = postgres.AddDatabase("postgresdb");
    
    builder.Services.AddSerilog();
    
    var migrations = builder.AddProject<Projects.Pathfinder_MigrationService>("migrations")
        .WithReference(postgresdb)
        .WaitFor(postgresdb);

    builder.AddProject<Projects.Pathfinder_Server>("api")
        .WithReference(postgresdb)
        .WithReference(migrations)
        .WaitFor(migrations);
    
    
    builder.AddNpmApp("reactvite", "../PathFinder.client");

    Log.Information("Building and running the application");
    builder.Build().Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}