using BookService.Application.Commands;
using BookService.Application.DTOs;
using BookService.Application.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Controllers
{
    [Route("books")]
    [ApiController]
    public class BookController: ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<BookRequestDTO> _validator;

        public BookController(IMediator mediator, IValidator<BookRequestDTO> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookRequestDTO requestDTO, CancellationToken cancellationToken)
        { 
            var command = new CreateBookCommand { BookRequestDTO = requestDTO };

            var createdBook = await _mediator.Send(command, cancellationToken);

            if (createdBook is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create book");
            }

            return StatusCode(StatusCodes.Status201Created, createdBook.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBook([FromRoute]int id, CancellationToken cancellationToken)
        {
            var query = new GetBookQuery { Id = id };
            
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks(CancellationToken cancellationToken)
        {
            var query = new GetBooksQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id, CancellationToken cancellationToken)
        {
            var command = new DeleteBookCommand { Id = id };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}