namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class EventsGroupsService : IEventsGroupsService
    {
        private readonly IRepository<EventGroup> repository;

        public EventsGroupsService(IRepository<EventGroup> repository)
        {
            this.repository = repository;
        }

        public async Task CreateEventGroupAsync(string eventId, string groupId)
        {
            var eventGroup = new EventGroup() { EventId = eventId, GroupId = groupId };
         
            await repository.AddAsync(eventGroup);

            await repository.SaveChangesAsync();
        }

        public async Task DeleteEventGroupAsync(string eventId, string groupId)
        {
            var eventGroup = await repository
                .All()
                .Where(x => x.EventId == eventId && x.GroupId == groupId)
                .FirstOrDefaultAsync();

            repository.Delete(eventGroup);

            await repository.SaveChangesAsync();
        }
    }
}