using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Workers
{
    public class OutboxPublisherService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IPublishEndpoint _publishEndpoint;

        public OutboxPublisherService(IServiceScopeFactory scopeFactory, IPublishEndpoint publishEndpoint) 
        {
            _scopeFactory = scopeFactory;
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishUnsentEventsAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var unsentMessages = await db.OutboxMessages.Where(x => x.ProcessedOn == null).ToListAsync();

            foreach (var message in unsentMessages)
            {
                var eventType = Type.GetType(message.Type);

                if (eventType is null)
                    continue;

                var @event = JsonSerializer.Deserialize(message.Payload, eventType);
                if (@event is null)
                    continue;

                await _publishEndpoint.Publish(@event);

                message.ProcessedOn = DateTime.UtcNow;
            }

            await db.SaveChangesAsync();
        }
    }
}
