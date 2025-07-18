using LendingService.Domain.Entities;

namespace LendingService.Application.Interfaces
{
    public interface ILendingRepository
    {
        Task CreateAsync(Lending lending, CancellationToken cancellationToken);
    }
}
