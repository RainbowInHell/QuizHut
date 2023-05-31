namespace QuizHut.Infrastructure.EntityViewModels.Quizzes
{
    using AutoMapper;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;
    using System.Linq;

    public class QuizSimpleViewModel : IMapFrom<Quiz>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int QuestionsCount { get; set; }

        public string TimeToTake { get; set; }

        public bool IsPassed { get; set; } // New property

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Quiz, QuizSimpleViewModel>()
                .ForMember(
                    x => x.QuestionsCount,
                    opt => opt.MapFrom(x => x.Questions.Count))
                .ForMember(
                    x => x.TimeToTake,
                    opt => opt.MapFrom(x => x.Timer != null ? $"{x.Timer} минут" : "Не ограничен по времени"))
                    .ForMember(x => x.IsPassed, opt => opt.MapFrom(r => r.Results.Any()));
        }
    }
}