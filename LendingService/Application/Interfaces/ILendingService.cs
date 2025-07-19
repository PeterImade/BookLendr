using Contracts.Results;
using LendingService.Application.Dtos;

namespace LendingService.Application.Interfaces
{
    public interface ILendingService
    {

        Task<Result<LendingResponse>> LendAsync(LendRequest lendRequest, int userId, CancellationToken cancellationToken);
        Task<bool> CheckBookAvailability(int bookId, CancellationToken cancellationToken);
    }
}
