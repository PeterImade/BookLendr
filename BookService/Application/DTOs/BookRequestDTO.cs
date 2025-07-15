namespace BookService.Application.DTOs
{
    public class BookRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
    }
}
