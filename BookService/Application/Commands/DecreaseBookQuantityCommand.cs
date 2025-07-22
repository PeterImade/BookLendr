using BookService.Application.Interfaces;
using Contracts.Results;
using MediatR;

namespace BookService.Application.Commands
{
    public class DecreaseBookQuantityCommand: IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DecreaseBookQuantityCommandHandler: IRequestHandler<DecreaseBookQuantityCommand, Result>
    {
        private readonly IBookBLService _bookBLService;
        private readonly IBookRepository _bookRepository;

        public DecreaseBookQuantityCommandHandler(IBookBLService bookBLService, IBookRepository bookRepository)
        {
            _bookBLService = bookBLService;
            _bookRepository = bookRepository;
        }

        public async Task<Result> Handle(DecreaseBookQuantityCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);

            if (book is null)
            {
                return Result.Failed("Book does not exist");
            }

            if (book.Quantity < 1)
            {
                return Result.Failed("Book is not available");
            }

            book.Quantity--;

            await _bookRepository.UpdateAsync(book, cancellationToken);

            return Result.Success();
        }
    }
}
