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
            var createdBook = await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync(cancellationToken);
            return createdBook.Entity;
        }

        public async Task<Book?> GetByIdAsync(int id, CancellationToken? cancellationToken)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id); 
            return book;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken)
        {
            var books = await _context.Books.AsNoTracking().ToListAsync(cancellationToken);

            return books;
        }

        public async Task DeleteAsync(Book bookToDelete, CancellationToken cancellationToken)
        {
            _context.Books.Remove(bookToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<Book> UpdateAsync(Book bookToUpdate, CancellationToken? cancellationToken)
        {
            var updatedBook = _context.Books.Update(bookToUpdate);
            await _context.SaveChangesAsync();
            return updatedBook.Entity;
        }
    }
}
