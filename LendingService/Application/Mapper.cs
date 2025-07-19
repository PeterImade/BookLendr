using LendingService.Application.Dtos;
using LendingService.Domain.Entities;

namespace LendingService.Application
{
    public static class Mapper
    {
        public static LendingResponse MapToDto(Lending lending)
        {
            return new LendingResponse
            {
                Id = lending.Id,
                BookId = lending.BookId,
                UserId = lending.UserId,
                DueDate = lending.DueDate,
                LendDate = lending.LendDate,
            };
        }
    }
}
