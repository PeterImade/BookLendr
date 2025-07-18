namespace BookService.Application.DTOs
{
    public class BookResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool IsAvailable => Quantity > 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
