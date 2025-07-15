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
                ISBN = requestDTO.ISBN
            };
        }
        public static Book ToUpdatedEntity(BookUpdateRequestDTO updateRequestDTO)
        {
            return new Book
            {
                Id = updateRequestDTO.Id,
                Title = updateRequestDTO.Title,
                Author = updateRequestDTO.Author,
                ISBN = updateRequestDTO.ISBN
            };
        }

        public static BookResponseDTO ToDTO(Book book)
        {
            return new BookResponseDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                IsAvailable = book.IsAvailable,
                CreatedAt = book.CreatedAt
            };
        }
    }
}
