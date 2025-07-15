using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using Contracts.Results;
using MediatR;

namespace BookService.Application.Commands
{
    public class CreateBookCommand: IRequest<Result<BookResponseDTO>>
    {
        public required BookRequestDTO BookRequestDTO { get; set; }
    }

    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Result<BookResponseDTO>>
    {
        private readonly IBookBLService _bookService;

        public CreateBookCommandHandler(IBookBLService bookService)
        {
            _bookService = bookService;
        }
        public async Task<Result<BookResponseDTO>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var createdBook = await _bookService.CreateBookAsync(request.BookRequestDTO, cancellationToken);

            return createdBook;
        }
    }
}
