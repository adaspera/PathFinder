using System.Net.Http.Headers;
using PathFinder.Server.Services;
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

    app.UseDefaultFiles();
    app.UseStaticFiles();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors();

    app.UseAuthorization();

    app.MapControllers();

    app.MapFallbackToFile("/index.html");

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