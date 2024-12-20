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

        try {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "product_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Please make sure that RabbitMQ is running.");
        }
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

        try {
            _channel.BasicConsume(queue: "product_queue", autoAck: true, consumer: consumer);
        } catch {
            Console.WriteLine("Please make sure that RabbitMQ is running.");
        }

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
        try {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        } catch {
            Console.WriteLine("Please make sure that RabbitMQ is running.");
        }
    }
}