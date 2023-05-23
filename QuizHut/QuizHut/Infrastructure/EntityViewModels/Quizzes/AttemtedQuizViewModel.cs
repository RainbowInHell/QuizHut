namespace QuizHut.Infrastructure.EntityViewModels.Quizzes
{
    using System.Collections.Generic;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Questions;

    public class AttemtedQuizViewModel : IMapFrom<Quiz>
    {
        public AttemtedQuizViewModel()
        {
            Questions = new List<AttemtedQuizQuestionViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Timer { get; set; }

        public IList<AttemtedQuizQuestionViewModel> Questions { get; set; }
    }
}
