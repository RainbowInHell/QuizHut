namespace QuizHut.Infrastructure.EntityViewModels.Events
{
    using System;
    using System.Collections.Generic;
    
    using AutoMapper;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;

    public class StudentPendingEventViewModel : IMapFrom<Event>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string QuizName { get; set; }

        public string Date { get; set; }

        public string Duration { get; set; }

        public DateTime ActivationDateAndTime { get; set; }

        public TimeSpan DurationOfActivity { get; set; }

        public IList<QuizSimpleViewModel> Quizzes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Event, StudentActiveEventViewModel>()
                .ForMember(x => x.Quizzes, opt => opt.MapFrom(e => e.Quizzes));
        }
    }
}