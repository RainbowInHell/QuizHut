namespace QuizHut.Infrastructure.EntityViewModels.Events
{
    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;

    public class EventsAssignViewModel : IMapFrom<Event>
    {
        public string Id { get; set; }

        public string CreatorId { get; set; }

        public string Name { get; set; }

        public bool IsAssigned { get; set; }
    }
}