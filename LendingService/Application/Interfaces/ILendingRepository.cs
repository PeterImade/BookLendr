using LendingService.Domain.Entities;

namespace LendingService.Application.Interfaces
{
    public interface ILendingRepository
    {
        Task<Lending> CreateAsync(Lending lending, CancellationToken cancellationToken);
        Task<IEnumerable<Lending>> GetAllLendingsAsync(CancellationToken cancellationToken);
        Task<Lending?> GetLendingAsync(int id, CancellationToken cancellationToken);
        Task<Lending?> GetLendingByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<Lending> UpdateAsync(Lending lending, CancellationToken cancellationToken);
    }
}
