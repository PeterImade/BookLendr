using Contracts.Events;
using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Services;

namespace NotificationService.Application.Consumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly IEmailService _emailService;

        public UserRegisteredConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        { 
            var message = context.Message;

            var subject = "Welcome to Book Lendr!";

            var body = $"Hi {message.FullName}, welcome to our app";

            await _emailService.SendEmailAsync(message.Email, subject, body); 
        }
    }
}