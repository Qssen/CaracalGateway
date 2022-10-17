using CaracalGateway.DelegatingHandlers;
using CaracalGateway.Infrastructure;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Configuration.AddJsonFile("gatewayConfig.json");
builder
    .Services
    .AddTransient<IKeyValueStorage, RedisKeyValueStorage>()
    .AddTransient<ITokenService, TokenService>()
    .AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(x =>
    {
        var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "redis";
        var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT") ?? "6379";
        var redisPassword = Environment.GetEnvironmentVariable("REDIS_PASSWORD");

        var redisOptions = ConfigurationOptions.Parse($"{redisHost}:{redisPort}");
        if (!string.IsNullOrWhiteSpace(redisPassword))
        {
            redisOptions.Password = redisPassword;
        }
        return ConnectionMultiplexer.Connect(redisOptions);
    })
    .AddOcelot()
    .AddDelegatingHandler<DomainLimitDelegatingHandler>()
    .AddDelegatingHandler<EndpointApiTokenCheckDelegatingHandler>()
    .AddDelegatingHandler<TestRequestDelegationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseOcelot().Wait();

app.MapControllers();

app.Run();
