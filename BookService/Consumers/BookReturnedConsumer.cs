using BookService.Application;
using BookService.Application.Interfaces;
using Contracts.Events;
using MassTransit;

namespace BookService.Consumers
{
    public class BookReturnedConsumer : IConsumer<BookReturnedEvent>
    {
        private readonly IBookBLService _bookBLService;
        private readonly IBookRepository _bookRepository;

        public BookReturnedConsumer(IBookBLService bookBLService, IBookRepository bookRepository)
        {
            _bookBLService = bookBLService;
            _bookRepository = bookRepository;
        }
        public async Task Consume(ConsumeContext<BookReturnedEvent> context)
        { 
            var bookId = context.Message.BookId;

            var book = await _bookBLService.GetBookAsync(bookId, null);

            if (book.Value != null)
            {
                book.Value.Quantity += 1;
                
                var bookToUpdate = Mapper.ToBookEntity(book.Value);

                await _bookRepository.UpdateAsync(bookToUpdate, null);
            }
        }
    }
}
