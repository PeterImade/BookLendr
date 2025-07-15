using BookService.Application.DTOs;
using FluentValidation;

namespace BookService.Application.Validators
{
    public sealed class BookValidator: AbstractValidator<BookRequestDTO>
    {
        public BookValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is a required field");
            RuleFor(x => x.Author).NotEmpty().WithMessage("Author is a required field");
            RuleFor(x => x.ISBN).NotEmpty().WithMessage("ISBN is a required field");
        }
    }
}
