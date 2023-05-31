namespace QuizHut.Infrastructure.EntityViewModels.Results
{
    using AutoMapper;
    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;

    public class ScoreViewModel : IMapFrom<Result>, IHaveCustomMappings
    {
        public string EventId { get; set; }

        public string Score { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Result, ScoreViewModel>()
               .ForMember(
                   x => x.Score,
                   opt => opt.MapFrom(x => $"{x.Points}/{x.MaxPoints}"));
        }
    }
}