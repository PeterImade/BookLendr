using MediatR;
using UserService.Application.DTOs;
using UserService.Infrastructure.Services;

namespace UserService.Application.Queries
{
    public class GetUserByEmailQuery: IRequest<UserResponseDTO>
    {
        public required string Email { get; set; }
    }

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserResponseDTO>
    {
        private readonly UserBLService _userBLService;

        public GetUserByEmailQueryHandler(UserBLService userBLService)
        {
            _userBLService = userBLService;
        }
        public async Task<UserResponseDTO> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userBLService.GetUserByEmailAsync(request.Email, cancellationToken);

            return user;
        }
    }
}
