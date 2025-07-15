using BookService.Application.Interfaces;
using BookService.Domain.Entities;
using BookService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookService.Infrastructure.Repositories
{
    public class BookRepository: IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Book> CreateAsync(Book book, CancellationToken cancellationToken)
        {
            var createdBook = await _context.AddAsync(book);
            await _context.SaveChangesAsync(cancellationToken);
            return createdBook.Entity;
        }

        public async Task<Book?> GetByIdAsync(int id,CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(id, cancellationToken);
            return book;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken)
        {
            var books = await _context.Books.ToListAsync(cancellationToken);

            return books;
        }

        public async Task DeleteAsync(Book book, CancellationToken cancellationToken)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}
