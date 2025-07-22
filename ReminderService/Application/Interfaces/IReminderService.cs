namespace ReminderService.Application.Interfaces
{
    public interface IReminderService
    {
        Task SendReminderAsync(string userEmail, string bookTitle, DateTime dueDate);
    }
}
