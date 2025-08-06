using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Services;

namespace UserService.Application.Commands
{
    public class LoginUserCommand: IRequest<TokenDTO>
    {
        public required UserLoginDTO UserLoginDTO { get; set; }
    }

    public class LoginUserCommandHandler: IRequestHandler<LoginUserCommand, TokenDTO>
    {
        private readonly IUserService _userBLService;

        public LoginUserCommandHandler(IUserService userBLService)
        {
            _userBLService = userBLService;
        }
        public async Task<TokenDTO> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var tokens = await _userBLService.LoginUserAsync(request.UserLoginDTO, cancellationToken);

            return tokens;
        }
    }
}
