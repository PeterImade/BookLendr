using UserService.Application.DTOs;
using UserService.Domain.Entities;
using UserService.Exceptions;
using UserService.Infrastructure.Repositories;

namespace UserService.Services
{
    public class UserBLService
    {
        private readonly UserRepository _userRepository;

        public UserBLService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        } 

        public async Task<UserResponseDTO> RegisterUserAsync(UserRegisterDTO registerDTO, CancellationToken cancellationToken)
        {
            var userExists = await _userRepository.CheckUserExists(registerDTO.Email, cancellationToken);

            if (userExists)
                throw new BadRequestException("Email is already taken");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            var user = new User
            {
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PasswordHash = hashedPassword
            };

            var createdUser = await _userRepository.CreateUser(user, cancellationToken);

            var userResponseDTO = new UserResponseDTO 
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName
            };

            return userResponseDTO;
        }

        public async Task<UserResponseDTO> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(email, cancellationToken);

            if (user is null)
                throw new NotFoundException($"User with email:{email} not found");

            var userResponseDTO = new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return userResponseDTO;
        }

        public async Task<UserResponseDTO> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(id, cancellationToken);

            if (user is null)
                throw new NotFoundException($"User with id:{id} not found");

            var userResponseDTO = new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return userResponseDTO;
        }
    }
}
