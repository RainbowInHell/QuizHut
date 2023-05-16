namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class EventsGroupsService : IEventsGroupsService
    {
        private readonly IDeletableEntityRepository<EventGroup> repository;

        public EventsGroupsService(IDeletableEntityRepository<EventGroup> repository)
        {
            this.repository = repository;
        }

        public async Task CreateEventGroupAsync(string eventId, string groupId)
        {
            var deletedEventGroup = await repository
                .All()
                .Where(x => x.GroupId == groupId && x.EventId == eventId)
                .FirstOrDefaultAsync();

            if (deletedEventGroup != null)
            {
                repository.Undelete(deletedEventGroup);
            }
            else
            {
                var eventGroup = new EventGroup() { EventId = eventId, GroupId = groupId };
                await repository.AddAsync(eventGroup);
            }

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string eventId, string groupId)
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