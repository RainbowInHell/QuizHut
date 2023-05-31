namespace QuizHut.Infrastructure.EntityViewModels.Results
{
    using System;

    using AutoMapper;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;

    public class ScoreViewModel : IMapFrom<Result>, IHaveCustomMappings
    {
        public string QuizId { get; set; }

        public string Score { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Result, ScoreViewModel>()
               .ForMember(
                   x => x.Score,
                   opt => opt.MapFrom(x => $"{Math.Round(x.Points, 2)}/{x.MaxPoints}"));
        }
    }
}