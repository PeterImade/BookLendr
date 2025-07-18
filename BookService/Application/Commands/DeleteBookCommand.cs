using BookService.Application.Interfaces;
using Contracts.Results;
using MediatR;

namespace BookService.Application.Commands
{
    public class DeleteBookCommand: IRequest<Result>
    {
        public required int Id { get; set; }
    }

    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Result>
    {
        private readonly IBookBLService _bookBLService;

        public DeleteBookCommandHandler(IBookBLService bookBLService)
        {
            _bookBLService = bookBLService;
        }
        public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
           return await _bookBLService.DeleteBookAsync(request.Id, cancellationToken);
        }
    }
}
