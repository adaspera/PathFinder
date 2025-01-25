using System.Net.Http.Headers;
using PathFinder.Server.Services;
using PathFinder.Server.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

builder.Services.AddHttpClient<MobilityDbTokenService>((client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["MobilityDb:BaseUrl"]);
});

builder.Services.AddSingleton<MobilityDbTokenService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(nameof(MobilityDbTokenService));
    var refreshToken = sp.GetRequiredService<IConfiguration>()["MobilityDb:RefreshToken"];
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

// Configure the HTTP request pipeline.
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

