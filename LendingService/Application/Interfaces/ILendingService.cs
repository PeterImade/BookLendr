using Contracts.Results;
using LendingService.Application.Dtos;

namespace LendingService.Application.Interfaces
{
    public interface ILendingService
    {

        Task<Result<LendingResponse>> LendAsync(LendRequest lendRequest, int userId, string userEmail, CancellationToken cancellationToken);
        Task<(bool, string?)> CheckBookAvailability(int bookId, CancellationToken cancellationToken);
        Task<Result<IEnumerable<LendingResponse>>> GetLendingsAsync(CancellationToken cancellationToken);
        Task<Result<LendingResponse>> GetLendingAsync(int id, CancellationToken cancellationToken);
        Task<Result<LendingResponse>> GetLendingByUserIdAsync(int userId, CancellationToken cancellationToken);
    }
}
