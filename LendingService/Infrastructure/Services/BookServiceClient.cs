namespace LendingService.Infrastructure.Services
{
    public class BookServiceClient
    {
        private readonly HttpClient httpClient;

        public BookServiceClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> DecreaseBookQuantityAsync(int bookId)
        {
            var response = await httpClient.PutAsync($"books/{bookId}/decrease-quantity", null);
            return response.IsSuccessStatusCode;
        }
    }
}
