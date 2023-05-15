using AutoMapper;
using QuizHut.BLL.MapperConfig.Contracts;
using QuizHut.DLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizHut.Infrastructure.EntityViewModels.Quizzes
{
    public class QuizListViewModel : IMapFrom<Quiz>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int QuestionsCount { get; set; }

        public string CategoryName { get; set; }

        public string CreatedOnDate { get; set; }

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
                    opt => opt.MapFrom(x => x.EventId != null ? "#0f990f" : "#d75277"));
        }
    }
}