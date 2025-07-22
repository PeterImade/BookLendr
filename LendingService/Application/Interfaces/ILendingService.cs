using Contracts.Results;
using LendingService.Application.Dtos;

namespace LendingService.Application.Interfaces
{
    public interface ILendingService
    {

        Task<Result<LendingResponse>> LendAsync(LendRequest lendRequest, int userId, string userEmail, CancellationToken cancellationToken);
        Task<(bool, string?)> CheckBookAvailability(int bookId, CancellationToken cancellationToken);
    }
}
