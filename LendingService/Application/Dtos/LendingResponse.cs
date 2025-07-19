using LendingService.Domain.Entities;

namespace LendingService.Application.Dtos
{
    public record LendingResponse
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime LendDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public Status Status { get; set; }
    }
}
