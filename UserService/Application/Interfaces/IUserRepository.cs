using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user, CancellationToken cancellationToken);
        Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken);
        Task<User?> GetUserById(int id, CancellationToken cancellationToken);
        Task<bool> CheckUserExists(string email, CancellationToken cancellationToken);
        Task SaveRefreshToken(User user, string refreshToken, DateTime refreshTokenExpiryTime, CancellationToken cancellationToken);
    }
}