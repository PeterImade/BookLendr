using BookService.Application;
using BookService.Application.DTOs;
using BookService.Application.Interfaces; 
using Contracts.Results;

namespace BookService.Infrastructure.Services
{
    public class BookBLService: IBookBLService
    {
        private readonly IBookRepository _bookRepository;

        public BookBLService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Result<BookResponseDTO>> CreateBookAsync(BookRequestDTO bookRequestDTO, CancellationToken cancellationToken)
        {
            var book = Mapper.ToEntity(bookRequestDTO);

            var createdBook = await _bookRepository.CreateAsync(book, cancellationToken);
             
            var bookResponseDTO = Mapper.ToDTO(book);

            return Result<BookResponseDTO>.Success(bookResponseDTO);
        }

        public async Task<Result> DeleteBookAsync(int id, CancellationToken cancellationToken)
        {
            var bookToDelete = await _bookRepository.GetByIdAsync(id, cancellationToken);

            if (bookToDelete is null)
                return Result.Failed($"Book with the id: {id} not found!");

            await _bookRepository.DeleteAsync(bookToDelete, cancellationToken);

            return Result.Success();
        }

        public async Task<Result<BookResponseDTO>> GetBookAsync(int id, CancellationToken cancellationToken)
        { 
            var book = await _bookRepository.GetByIdAsync(id, cancellationToken);

            if (book is null)
                return Result<BookResponseDTO>.Failed($"Book with the id: {id} not found!");

            var bookResponseDTO = Mapper.ToDTO(book);

            return Result<BookResponseDTO>.Success(bookResponseDTO);
        }

        public async Task<Result<IEnumerable<BookResponseDTO>>> GetBooksAsync(CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetBooksAsync(cancellationToken);

            var response = books.Select(x => Mapper.ToDTO(x));

            return Result<IEnumerable<BookResponseDTO>>.Success(response);
        }

        public async Task<Result<BookResponseDTO>> UpdateBookAsync(BookUpdateRequestDTO bookUpdateRequestDTO, CancellationToken cancellationToken)
        {
            if (bookUpdateRequestDTO is null)
            {
                return Result<BookResponseDTO>.Failed("Request is null");
            }

            var existingBook = await _bookRepository.GetByIdAsync(bookUpdateRequestDTO.Id, cancellationToken);

            if (existingBook is null)
            {
                return Result<BookResponseDTO>.Failed("Book not found");
            }

            var bookToUpdate = Mapper.ToUpdatedEntity(bookUpdateRequestDTO);

            var updatedBook = await _bookRepository.UpdateAsync(bookToUpdate, cancellationToken);

            var result = Mapper.ToDTO(updatedBook);

            return Result<BookResponseDTO>.Success(result);
        }
    }
}
