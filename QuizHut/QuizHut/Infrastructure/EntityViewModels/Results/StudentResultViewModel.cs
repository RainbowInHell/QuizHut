namespace QuizHut.Infrastructure.EntityViewModels.Results
{
    using System;

    using AutoMapper;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;

    public class StudentResultViewModel : IMapFrom<Result>, IHaveCustomMappings
    {
        public string EventName { get; set; }

        public string QuizName { get; set; }

        public string Date { get; set; }

        public string TimeSpent { get; set; }

        public string Score { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Result, StudentResultViewModel>()
                .ForMember(
                  x => x.Date,
                  opt => opt.MapFrom(x => x.Quiz.Event.ActivationDateAndTime.ToShortDateString()))
              .ForMember(
                  x => x.EventName,
                  opt => opt.MapFrom(x => x.Quiz.Event.Name))
              .ForMember(
                  x => x.QuizName,
                  opt => opt.MapFrom(x => x.Quiz.Name))
              .ForMember(
                   x => x.TimeSpent,
                   opt => opt.MapFrom(x => x.TimeSpent.ToString(@"hh\:mm\:ss")))
              .ForMember(
                  x => x.Score,
                  opt => opt.MapFrom(x => $"{Math.Round(x.Points, 2)}/{x.MaxPoints}"));
        }
    }
}