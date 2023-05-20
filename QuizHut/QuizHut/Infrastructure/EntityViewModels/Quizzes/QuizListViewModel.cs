namespace QuizHut.Infrastructure.EntityViewModels.Quizzes
{
    using System;

    using AutoMapper;
    
    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;

    public class QuizListViewModel : IMapFrom<Quiz>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int QuestionsCount { get; set; }

        public string CategoryName { get; set; }

        public string CreatedOnDate { get; set; }

        public string Description { get; set; }

        public string Password { get; set; }

        public int Timer { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Color { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Quiz, QuizListViewModel>()
                .ForMember(
                    x => x.QuestionsCount,
                    opt => opt.MapFrom(x => x.Questions.Count))
                .ForMember(
                    x => x.Color,
                    opt => opt.MapFrom(x => x.EventId != null ? "#0f990f" : "#d75277"))
                .ForMember(
                    x => x.Password,
                    opt => opt.MapFrom(x => x.Password.Content));
        }
    }
}