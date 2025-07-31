using System.Net.Http.Headers;
using PathFinder.Server.Data;
using PathFinder.Server.Models;
using PathFinder.Server.Repositories;
using PathFinder.Server.Repositories.Interfaces;
using PathFinder.Server.Services;
using PathFinder.Server.Services.Interfaces;
using PathFinder.Server.Utils;
using Serilog;
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
    
    var builder = WebApplication.CreateBuilder(args);
    
    builder.AddNpgsqlDbContext<AppDbContext>(connectionName: "postgresdb");
    
    builder.Services.AddScoped<IStopRepository, StopRepository>();
    builder.Services.AddScoped<IStopService, StopService>();
    builder.Services.AddScoped<IRouteRepository, RouteRepository>();
    builder.Services.AddScoped<IRouteService, RouteService>();
    builder.Services.AddScoped<IFeedInfoRepository, FeedInfoRepository>();
    
    builder.Services.AddScoped<CitySearchService>(provider => 
        new CitySearchService(Path.Combine(Environment.CurrentDirectory, "LuceneSearch")));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    builder.Services.AddSingleton<MobilityDbTokenService>(sp =>
    {
        var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient(nameof(MobilityDbTokenService));
        httpClient.BaseAddress = new Uri(builder.Configuration["MobilityDb:BaseUrl"]);
        var refreshToken = builder.Configuration["MobilityDb:RefreshToken"];
        return new MobilityDbTokenService(httpClient, refreshToken);
    });


    builder.Services.AddHttpClient<MobilityDbService>((client) =>
    {
        client.BaseAddress = new Uri(builder.Configuration["MobilityDb:BaseUrl"]);
    }).AddHttpMessageHandler(serviceProvider =>
    {
        var tokenService = serviceProvider.GetRequiredService<MobilityDbTokenService>();
        return new TokenRefreshHandler(tokenService);
    });

    var app = builder.Build();
    

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors();

    app.UseAuthorization();

    app.MapControllers();
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}