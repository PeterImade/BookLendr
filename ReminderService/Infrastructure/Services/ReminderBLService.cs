using Contracts.Events;
using MassTransit;
using ReminderService.Application.Interfaces;

namespace ReminderService.Infrastructure.Services
{
    public class ReminderBLService: IReminderService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ReminderBLService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public Task SendReminderAsync(string userEmail, string bookTitle, DateTime dueDate)
        {
            _publishEndpoint.Publish(new ReminderEmailEvent
            {
                UserEmail = userEmail,
                BookTitle = bookTitle,
                DueDate = dueDate
            });

            return Task.CompletedTask;
        }
    }
}