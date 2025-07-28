using UserService.Application.DTOs;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> RegisterUserAsync(UserRegisterDTO registerDTO, CancellationToken cancellationToken);
        Task<TokenDTO> LoginUserAsync(UserLoginDTO userLoginDTO, CancellationToken cancellationToken);
        Task<UserResponseDTO> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<UserResponseDTO> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    }
}
