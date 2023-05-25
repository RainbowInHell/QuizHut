namespace QuizHut.Infrastructure.EntityViewModels.Events
{
    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.DLL.Entities;

    public class EventSimpleViewModel : IMapFrom<Event>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Status Status { get; set; }
    }
}