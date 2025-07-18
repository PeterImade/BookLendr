using BookService.Application.DTOs;
using BookService.Domain.Entities;

namespace BookService.Application
{
    public static class Mapper
    {
        public static Book ToEntity(BookRequestDTO requestDTO)
        {
            return new Book
            {
                Title = requestDTO.Title,
                Author = requestDTO.Author,
                Quantity = requestDTO.Quantity,
                ISBN = requestDTO.ISBN
            };
        }
        public static void MapToExistingEntity(Book entity, BookUpdateRequestDTO bookUpdateRequestDTO)
        {
            entity.Title = bookUpdateRequestDTO.Title;
            entity.ISBN = bookUpdateRequestDTO.ISBN;
            entity.Quantity = bookUpdateRequestDTO.Quantity;
            entity.Author = bookUpdateRequestDTO.Author; 
        }

        public static BookResponseDTO ToDTO(Book book)
        {
            return new BookResponseDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Quantity = book.Quantity,
                CreatedAt = book.CreatedAt
            };
        }
    }
}
