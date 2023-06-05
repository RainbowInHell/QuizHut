namespace QuizHut.Infrastructure.EntityViewModels.Events
{
    using System;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.DLL.Entities;

    public class EventListViewModel : IMapFrom<Event>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime ActivationDateAndTime { get; set; }

        public TimeSpan DurationOfActivity { get; set; }

        public string StartDate { get; set; }

        public string Duration { get; set; }

        public Status Status { get; set; }
    }
}