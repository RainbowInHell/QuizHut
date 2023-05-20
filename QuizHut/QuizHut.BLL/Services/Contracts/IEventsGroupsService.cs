namespace QuizHut.BLL.Services.Contracts
{
    public interface IEventsGroupsService
    {
        Task CreateEventGroupAsync(string eventId, string groupId);

        Task DeleteEventGroupAsync(string eventId, string groupId);
    }
}