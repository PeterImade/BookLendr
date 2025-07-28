using Contracts.Events;
using MassTransit;
using MediatR;
using System.Text.Json;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Exceptions;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Services;

namespace UserService.Application.Commands
{
    public class CreateUserCommand: IRequest<UserResponseDTO>
    {
        public required UserRegisterDTO UserRegisterDTO { get; set; }
    }

    public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, UserResponseDTO>
    {
        private readonly IUserRepository _userRepository; 
        private readonly IUserService _userService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ApplicationDbContext _dbContext;

        public CreateUserCommandHandler(IUserRepository userRepository, IUserService userService, IPublishEndpoint publishEndpoint, ApplicationDbContext dbContext)
        {
            _userRepository = userRepository;
            _userService = userService;
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
        }
        public async Task<UserResponseDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
                var userExists = await _userRepository.CheckUserExists(request.UserRegisterDTO.Email, cancellationToken);

                if (userExists)
                    throw new BadRequestException("Email is already taken");

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.UserRegisterDTO.Password);

                var user = new User
                {
                    FirstName = request.UserRegisterDTO.FirstName,
                    LastName = request.UserRegisterDTO.LastName,
                    Email = request.UserRegisterDTO.Email,
                    PasswordHash = hashedPassword
                };

                await _dbContext.Users.AddAsync(user, cancellationToken);

                var @event = new UserRegisteredEvent
                {
                    Id = user.Id,
                    Email = request.UserRegisterDTO.Email,
                    FullName = $"{user.FirstName} {user.LastName}",
                };

                var payload = JsonSerializer.Serialize(@event);

                var outbox = new OutboxMessage
                {
                    Type = nameof(UserRegisteredEvent),
                    Payload = payload,
                    OccurredOn = DateTime.UtcNow
                };

                await _dbContext.OutboxMessages.AddAsync(outbox, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                var response = Mapper.ToDTO(user);

                return response; 
        }
    }
}
