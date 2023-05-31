namespace QuizHut.Infrastructure.EntityViewModels.Events
{
    using System;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;

    public class StudentPendingEventViewModel : IMapFrom<Event>
    {
        public string Name { get; set; }

        public string QuizName { get; set; }

        public string Date { get; set; }

        public string Duration { get; set; }

        public DateTime ActivationDateAndTime { get; set; }

        public TimeSpan DurationOfActivity { get; set; }
    }
}