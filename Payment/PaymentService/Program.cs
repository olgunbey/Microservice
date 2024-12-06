using MassTransit;
using Microsoft.EntityFrameworkCore;
using PaymentService.Consumers;
using PaymentService.Database;
using Shared.Constants;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PaymentDbContext>(y => y.UseNpgsql("Server=localhost; Port=5432; Username=postgres; Password=payment123; Database=Payment"));
builder.Services.AddMassTransit<IBus>(y =>
{
    y.AddConsumer<StockReservedEventConsumer>();

    y.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetSection("AmqpCloudRegister")["Host"], conf =>
        {
            conf.Username(builder.Configuration.GetSection("AmqpCloudRegister")["Username"]!);
            conf.Password(builder.Configuration.GetSection("AmqpCloudRegister")["Password"]!);
        });

        configurator.ReceiveEndpoint(RabbitMqSettings.Payment_StockReservedEventQueue, opt => opt.ConfigureConsumer<StockReservedEventConsumer>(context));

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
