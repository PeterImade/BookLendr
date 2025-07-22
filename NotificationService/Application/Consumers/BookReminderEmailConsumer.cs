using Contracts.Events;
using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Services;

namespace NotificationService.Application.Consumers
{
    public class BookReminderEmailConsumer : IConsumer<ReminderEmailEvent>
    {
        private readonly IEmailService _emailService;

        public BookReminderEmailConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task Consume(ConsumeContext<ReminderEmailEvent> context)
        {
            var message = context.Message;

            var subject = "Book Reminder";

            var body = $"Hi there! Just a gentle reminder that your borrowed book, '{message.BookTitle}', is due back on {message.DueDate.ToLongDateString()}. We'd love to have it returned soon. Thank you!";

            await _emailService.SendEmailAsync(message.UserEmail, subject, body);
        }
    }
}