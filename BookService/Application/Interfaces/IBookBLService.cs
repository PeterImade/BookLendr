
using BookService.Application.DTOs;
using Contracts.Results;

namespace BookService.Application.Interfaces
{
    public interface IBookBLService
    {
        Task<Result<BookResponseDTO>> CreateBookAsync(BookRequestDTO bookRequestDTO, CancellationToken cancellationToken);
        Task<Result<BookResponseDTO>> GetBookAsync(int id, CancellationToken cancellationToken);
        Task<Result<IEnumerable<BookResponseDTO>>> GetBooksAsync(CancellationToken cancellationToken);
        Task<Result> DeleteBookAsync(int id, CancellationToken cancellationToken);
    }
}
