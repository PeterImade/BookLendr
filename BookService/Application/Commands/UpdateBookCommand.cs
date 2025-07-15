using BookService.Application.DTOs;
using BookService.Application.Interfaces;
using Contracts.Results;
using MediatR;

namespace BookService.Application.Commands
{
    public class UpdateBookCommand: IRequest<Result<BookResponseDTO>>
    {
        public required BookUpdateRequestDTO BookUpdateRequestDTO { get; set; }
    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Result<BookResponseDTO>>
    {
        private readonly IBookBLService _bookBLService;

        public UpdateBookCommandHandler(IBookBLService bookBLService)
        {
            _bookBLService = bookBLService;
        }
        public async Task<Result<BookResponseDTO>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var updatedBook = await _bookBLService.UpdateBookAsync(request.BookUpdateRequestDTO, cancellationToken);

            return updatedBook;
        }
    }
}
