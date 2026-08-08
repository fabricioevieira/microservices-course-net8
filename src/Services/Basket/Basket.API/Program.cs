using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter(new DependencyContextAssemblyCatalogCustom(typeof(Program).Assembly));
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var repository = provider.GetRequiredService<BasketRepository>();
//    var cache = provider.GetRequiredService<IDistributedCache>();
//    return new CachedBasketRepository(repository, cache);
//});

builder.Services.AddStackExchangeRedisCache(op =>
{
    op.Configuration = builder.Configuration.GetConnectionString("Redis")!;
    //op.InstanceName = "Basket_"; 
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();
app.UseExceptionHandler(options => { });

app.Run();
