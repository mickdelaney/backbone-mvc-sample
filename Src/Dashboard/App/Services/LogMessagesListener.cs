using System;
using System.Threading.Tasks;
using Core;
using Dashboard.App.Models;
using Dashboard.App.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dashboard.App.Services
{
    public class LogMessagesListener
    {
        readonly LogMessagesRepository _logMessagesRepository;
        readonly RabbitMqService _rabbitMqService;

        public LogMessagesListener(LogMessagesRepository logMessagesRepository, RabbitMqService rabbitMqService)
        {
            _logMessagesRepository = logMessagesRepository;
            _rabbitMqService = rabbitMqService;
        }

        public Task Init()
        {
            var factory = new TaskFactory();
            var task = factory.StartNew(HandleMessages);
            return task;
        }

        public void HandleMessages()
        {
            using (var channel = _rabbitMqService.CreateModel(AppConstants.ExchangeName, AppConstants.QueueName, AppConstants.RoutingKey))
            {
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(AppConstants.QueueName, true, consumer);

                while (true)
                {
                    var message = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                    var logMessage = new LogMessage
                    {
                        ArrivedAt = DateTime.Now,
                        Message = message.Body.AsUtf8String()
                    };

                    _logMessagesRepository.AddMessage(logMessage);

                    Console.Write(logMessage.Message);
                }
            }

        }

    }
}