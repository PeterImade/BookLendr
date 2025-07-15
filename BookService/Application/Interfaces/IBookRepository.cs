
using BookService.Domain.Entities;

namespace BookService.Application.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> CreateAsync(Book book, CancellationToken cancellationToken);
        Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken);
        Task DeleteAsync(Book book, CancellationToken cancellationToken);
    }
}
