using Contracts.Events;
using MassTransit;

namespace NotificationService.Application.Consumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        { 
            var message = context.Message;

            Console.WriteLine($"📨 Sending welcome email to {message.Email}...");

            // TODO: Inject your email service and call it here
            await Task.CompletedTask;
        }
    }
}
