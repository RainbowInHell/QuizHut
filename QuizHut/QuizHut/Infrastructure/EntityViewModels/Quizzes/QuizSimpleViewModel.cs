namespace QuizHut.Infrastructure.EntityViewModels.Quizzes
{
    using System.Linq;

    using AutoMapper;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Results;

    public class QuizSimpleViewModel : IMapFrom<Quiz>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int QuestionsCount { get; set; }

        public string TimeToTake { get; set; }

        public string Color { get; set; }

        public ScoreViewModel Score { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Quiz, QuizSimpleViewModel>()
                .ForMember(
                    x => x.QuestionsCount,
                    opt => opt.MapFrom(x => x.Questions.Count))
                .ForMember(
                    x => x.TimeToTake,
                    opt => opt.MapFrom(x => x.Timer != null ? $"{x.Timer} минут" : "Не ограничен по времени"))
                .ForMember(x => x.Color, opt => opt.MapFrom(r => r.Results.Any() ? "#0f990f" : "#d75277"));
        }
    }
}