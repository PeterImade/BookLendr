using UserService.Application.DTOs;
using UserService.Domain.Entities;
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

        public async Task RegisterUserAsync(UserRegisterDTO registerDTO)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            var user = new User
            {
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PasswordHash = hashedPassword
            };

            await _userRepository.CreateUser(user);
        }

        public async Task<UserResponseDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            var userResponseDTO = new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return userResponseDTO;
        }

        public async Task<UserResponseDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserById(id);

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
