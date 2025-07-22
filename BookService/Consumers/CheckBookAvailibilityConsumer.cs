using BookService.Application.Interfaces;
using Contracts.Events;
using MassTransit;

namespace BookService.Consumers
{
    public class CheckBookAvailabilityConsumer: IConsumer<CheckBookAvailabilityRequest>
    {
        private readonly IBookBLService _bookBLService;

        public CheckBookAvailabilityConsumer(IBookBLService bookBLService)
        {
            _bookBLService = bookBLService;
        }
        public async Task Consume(ConsumeContext<CheckBookAvailabilityRequest> context)
        {
            var bookId = context.Message.BookId;
            var book = await _bookBLService.GetBookAsync(bookId, null);

            bool isAvailable = book != null && book.Value?.Quantity > 0;

            if (!isAvailable)
            {
                await context.RespondAsync(new CheckBookAvailabilityResponse
                {
                    IsAvailable = isAvailable
                });

                return;
            }

            await context.RespondAsync(new CheckBookAvailabilityResponse
            {
                BookTitle = book.Value.Title,
                IsAvailable = isAvailable
            });
        }
    }
}