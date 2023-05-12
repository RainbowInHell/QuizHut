namespace QuizHut.BLL.Services.Contracts
{
    public interface IEventsGroupsService
    {
        Task CreateEventGroupAsync(string eventId, string groupId);

        Task DeleteAsync(string eventId, string groupId);
    }
}