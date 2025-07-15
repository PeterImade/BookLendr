using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using Contracts.Results;
using MediatR;

namespace BookService.Application.Queries
{
    public class GetBooksQuery: IRequest<Result<IEnumerable<BookResponseDTO>>>
    {
    }

    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, Result<IEnumerable<BookResponseDTO>>>
    {
        private readonly IBookBLService _bookBLService;

        public GetBooksQueryHandler(IBookBLService bookBLService)
        {
            _bookBLService = bookBLService;
        }
        public async Task<Result<IEnumerable<BookResponseDTO>>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var fetchedBooks = await _bookBLService.GetBooksAsync(cancellationToken);

            return fetchedBooks;
        }
    }
}