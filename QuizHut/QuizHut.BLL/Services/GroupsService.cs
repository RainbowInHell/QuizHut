namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;
    
    using QuizHut.BLL.Expression.Contracts;
    using QuizHut.BLL.MapperConfig;
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class GroupsService : IGroupsService
    {
        private readonly IDeletableEntityRepository<Group> repository;

        private readonly IStudentsGroupsService studentsGroupsService;
                
        private readonly IEventsGroupsService eventsGroupsService;

        private readonly IExpressionBuilder expressionBuilder;

        public GroupsService(
            IDeletableEntityRepository<Group> repository,
            IStudentsGroupsService studentsGroupsService,
            IEventsGroupsService eventsGroupsService,
            IExpressionBuilder expressionBuilder)
        {
            this.repository = repository;
            this.studentsGroupsService = studentsGroupsService;
            this.eventsGroupsService = eventsGroupsService;
            this.expressionBuilder = expressionBuilder;
        }

        public async Task<IList<T>> GetAllGroupsAsync<T>(string creatorId = null, string eventId = null, string searchText = null)
        {
            var query = repository.AllAsNoTracking();

            if (creatorId != null)
            {
                query = query.Where(x => x.CreatorId == creatorId);
            }

            if (eventId != null)
            {
                query = query.Where(x => !x.EventsGroups.Any(x => x.EventId == eventId));
            }

            if (searchText != null)
            {
                var filter = expressionBuilder.GetExpression<Group>("Name", searchText);
                query = query.Where(filter);
            }

            return await query.OrderByDescending(x => x.CreatedOn).To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByEventIdAsync<T>(string eventId)
        {
            return await repository
                .AllAsNoTracking()
                .Where(x => x.EventsGroups.Any(x => x.EventId == eventId))
                .To<T>()
                .ToListAsync();
        }

        public async Task<string> CreateGroupAsync(string name, string creatorId)
        {
            var group = new Group() { Name = name, CreatorId = creatorId };

            await repository.AddAsync(group);
            await repository.SaveChangesAsync();

            return group.Id;
        }

        public async Task AssignStudentsToGroupAsync(string groupId, IList<string> studentsIds)
        {
            foreach (var studentId in studentsIds)
            {
                await studentsGroupsService.CreateStudentGroupAsync(groupId, studentId);
            }
        }

        public async Task AssignEventsToGroupAsync(string groupId, IList<string> evenstIds)
        {
            foreach (var eventId in evenstIds)
            {
                await eventsGroupsService.CreateEventGroupAsync(eventId, groupId);
            }
        }

        public async Task UpdateNameAsync(string groupId, string newName)
        {
            var group = await repository
                .All()
                .Where(x => x.Id == groupId)
                .FirstOrDefaultAsync();

            group.Name = newName;
            repository.Update(group);

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string groupId)
        {
            var group = await repository
                .All()
                .Where(x => x.Id == groupId)
                .FirstOrDefaultAsync();

            repository.Delete(group);
            
            await repository.SaveChangesAsync();
        }

        public async Task DeleteStudentFromGroupAsync(string groupId, string studentId)
        {
            await studentsGroupsService.DeleteAsync(groupId, studentId);
        }

        public async Task DeleteEventFromGroupAsync(string groupId, string eventId)
        {
            await eventsGroupsService.DeleteAsync(eventId, groupId);
        }
    }
}