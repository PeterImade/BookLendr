using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using Contracts.Results;
using MediatR;

namespace BookService.Application.Queries
{
    public class GetBookQuery: IRequest<Result<BookResponseDTO>>
    {
        public int Id { get; set; }
    }

    public class GetBookQueryHandler: IRequestHandler<GetBookQuery, Result<BookResponseDTO>>
    {
        private readonly IBookBLService _bookBLService;

        public GetBookQueryHandler(IBookBLService bookBLService)
        {
            _bookBLService = bookBLService;
        }
        public async Task<Result<BookResponseDTO>> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var fetchedBook = await _bookBLService.GetBookAsync(request.Id, cancellationToken);

            return fetchedBook;
        }
    }
}
