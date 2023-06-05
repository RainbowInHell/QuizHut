namespace QuizHut.Infrastructure.EntityViewModels.Events
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Quizzes;

    public class StudentEndedEventViewModel : IMapFrom<Event>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string QuizName { get; set; }

        public DateTime ActivationDateAndTime { get; set; }

        public string Date { get; set; }

        public IList<QuizSimpleViewModel> Quizzes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Event, StudentActiveEventViewModel>()
                .ForMember(x => x.Quizzes, opt => opt.MapFrom(e => e.Quizzes));
        }
    }
}