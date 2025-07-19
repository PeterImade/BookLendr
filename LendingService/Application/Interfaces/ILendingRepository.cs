using LendingService.Domain.Entities;

namespace LendingService.Application.Interfaces
{
    public interface ILendingRepository
    {
        Task<Lending> CreateAsync(Lending lending, CancellationToken cancellationToken);
    }
}
