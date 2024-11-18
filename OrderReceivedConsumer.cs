using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using ss_inventory_microservice.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ss_inventory_microservice;
public class OrderReceivedConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IInventoryRepository _inventoryRepository;

    public OrderReceivedConsumer(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;

        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "orderQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await ProcessMessage(message);
        };

        _channel.BasicConsume(queue: "orderQueue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    private async Task ProcessMessage(string message)
    {
        var order = JsonConvert.DeserializeObject<OrderMessage>(message);
        var product = await _inventoryRepository.GetItemByIdAsync(order?.ProductId ?? "");
        if (product != null)
        {
            product.Quantity -= order?.Quantity ?? 0;
            await _inventoryRepository.UpdateItemQuantityAsync(product);
        }
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}