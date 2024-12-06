using MassTransit;
using MongoDB.Driver;
using Shared.Constants;
using StockService.Consumers;
using StockService.MongoDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<StockMongoService>();


#region Seed Data
//using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();

//StockMongoService orderMongoService = scope.ServiceProvider.GetService<StockMongoService>();
//var collection = orderMongoService.GetCollection();
//if (!collection.FindSync(y => true).Any())
//{
//    await collection.InsertOneAsync(new() { ProductId = 1, Amount = 200 });
//    await collection.InsertOneAsync(new() { ProductId = 2, Amount = 6000 });
//    await collection.InsertOneAsync(new() { ProductId = 3, Amount = 300 });
//    await collection.InsertOneAsync(new() { ProductId = 4, Amount = 9000 });
//}
#endregion


builder.Services.AddMassTransit<IBus>(y =>
{
    y.AddConsumer<OrderCreatedEventConsumer>();
    y.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetSection("AmqpCloudRegister")["Host"], conf =>
        {
            conf.Username(builder.Configuration.GetSection("AmqpCloudRegister")["Username"]!);
            conf.Password(builder.Configuration.GetSection("AmqpCloudRegister")["Password"]!);
        });
        configurator.ReceiveEndpoint(RabbitMqSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
