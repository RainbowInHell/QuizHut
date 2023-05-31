namespace QuizHut.Infrastructure.EntityViewModels.Events
{
    using System;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Results;

    public class StudentEndedEventViewModel : IMapFrom<Event>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string QuizName { get; set; }

        public DateTime ActivationDateAndTime { get; set; }

        public string Date { get; set; }

        public ScoreViewModel Score { get; set; }
    }
}