using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Consumers;
using OrderService.Database;
using Shared.Constants;
using Shared.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderDbContext>(y => y.UseSqlServer("Server=OLGUNBEY\\SQLEXPRESS;Database=OrderDatabase;Trusted_Connection=True; TrustServerCertificate=True;"));
builder.Services.AddMassTransit(y =>
{
    y.AddConsumer<PaymentCompletedEventConsumer>();
    y.AddConsumer<StockNotReservedEventConsumer>();
    y.AddConsumer<PaymentNotCompletedEventConsumer>();

    y.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetSection("AmqpCloudRegister")["Host"], conf =>
        {
            conf.Username(builder.Configuration.GetSection("AmqpCloudRegister")["Username"]!);
            conf.Password(builder.Configuration.GetSection("AmqpCloudRegister")["Password"]!);
        });
        configurator.ReceiveEndpoint(RabbitMqSettings.Order_PaymentCompletedEventQueue, options => options.ConfigureConsumer<PaymentCompletedEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.Order_StockNotCompletedEventQueue,options=>options.ConfigureConsumer<StockNotReservedEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.Order_PaymentNotCompletedEventQueue, options => options.ConfigureConsumer<PaymentNotCompletedEventConsumer>(context));
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
