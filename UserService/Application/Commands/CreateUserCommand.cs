using MediatR;
using UserService.Application.DTOs;
using UserService.Services;

namespace UserService.Application.Commands
{
    public class CreateUserCommand: IRequest<Unit>
    {
        public required UserRegisterDTO UserRegisterDTO { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
    {
        private readonly UserBLService _userBLService;

        public CreateUserCommandHandler(UserBLService userBLService)
        {
            _userBLService = userBLService;
        }
        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _userBLService.RegisterUserAsync(request.UserRegisterDTO, cancellationToken);

            return Unit.Value;
        }
    }
}
