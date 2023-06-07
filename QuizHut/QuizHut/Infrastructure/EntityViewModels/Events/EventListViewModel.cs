namespace QuizHut.Infrastructure.EntityViewModels.Events
{
    using System;

    using AutoMapper;
    
    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Common;
    using QuizHut.DLL.Entities;

    public class EventListViewModel : IMapFrom<Event>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime ActivationDateAndTime { get; set; }

        public TimeSpan DurationOfActivity { get; set; }

        public string StartDate { get; set; }

        public string Duration { get; set; }

        public string StatusAsString { get; set; }
        
        public Status Status { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Event, EventListViewModel>()
                .ForMember(x => x.StatusAsString, opt => opt.MapFrom(src => GetRussianStatusString(src.Status)));
        }

        private static string GetRussianStatusString(Status status)
        {
            switch (status)
            {
                case Status.Active:
                    return "Активно";
                case Status.Pending:
                    return "В ожидании";
                case Status.Ended:
                    return "Завершено";
                default:
                    return string.Empty;
            }
        }
    }
}