namespace LendingService.Application.Dtos
{
    public class LendRequest
    {
        public int BookId { get; set; }
    }

    public class ReturnRequest
    {
        public int LendingId { get; set; }
    }

}
