namespace QuizHut.BLL.Services.Contracts
{
    public interface IGroupsService
    {
        Task<IList<T>> GetAllGroupsAsync<T>(
            string creatorId = null,
            string eventId = null,
            string searchCriteria = null,
            string searchText = null);

        Task<IEnumerable<T>> GetAllByEventIdAsync<T>(string eventId);

        Task<T> GetGroupModelAsync<T>(string groupId);

        Task<T> GetEventsFirstGroupAsync<T>(string eventId);

        Task<string> CreateGroupAsync(string name, string creatorId);

        Task AssignStudentsToGroupAsync(string groupId, IList<string> studentsIds);

        Task AssignEventsToGroupAsync(string groupId, IList<string> eventsIds);

        Task UpdateNameAsync(string groupId, string newName);

        Task DeleteAsync(string groupId);

        Task DeleteStudentFromGroupAsync(string groupId, string studentId);

        Task DeleteEventFromGroupAsync(string groupId, string eventId);
    }
}