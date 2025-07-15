using BookService.Application.Interfaces;
using MediatR;

namespace BookService.Application.Commands
{
    public class DeleteBookCommand: IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
    {
        private readonly IBookBLService _bookBLService;

        public DeleteBookCommandHandler(IBookBLService bookBLService)
        {
            _bookBLService = bookBLService;
        }
        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            await _bookBLService.DeleteBookAsync(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}
