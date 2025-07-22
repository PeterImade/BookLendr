using Contracts.Events;
using Hangfire;
using MassTransit;
using ReminderService.Application.Interfaces;

namespace ReminderService.Consumers
{
    public class BookLentConsumer: IConsumer<BookLentEvent>
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public BookLentConsumer(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }
        public Task Consume(ConsumeContext<BookLentEvent> context)
        { 
            var message = context.Message;
            var dueDate = message.DueDate;
            var reminderDate = dueDate.AddDays(-2);

            _backgroundJobClient.Schedule<IReminderService>(service =>
            
                service.SendReminderAsync(message.UserEmail, message.BookTitle, dueDate),
                reminderDate - DateTime.UtcNow
            );
                return Task.CompletedTask;
        }
    }
}
