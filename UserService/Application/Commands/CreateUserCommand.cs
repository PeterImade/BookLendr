using Contracts.Events;
using MassTransit;
using MediatR;
using UserService.Application.DTOs; 
using UserService.Services;

namespace UserService.Application.Commands
{
    public class CreateUserCommand: IRequest<UserResponseDTO>
    {
        public required UserRegisterDTO UserRegisterDTO { get; set; }
    }

    public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, UserResponseDTO>
    {
        private readonly UserBLService _userBLService;
        private readonly IPublishEndpoint _publishEndpoint;


        public CreateUserCommandHandler(UserBLService userBLService, IPublishEndpoint publishEndpoint)
        {
            _userBLService = userBLService;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<UserResponseDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userBLService.RegisterUserAsync(request.UserRegisterDTO, cancellationToken);

            await _publishEndpoint.Publish(new UserRegisteredEvent
            {
                Id = user.Id,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
            }, cancellationToken);

            return user;
        }
    }
}
