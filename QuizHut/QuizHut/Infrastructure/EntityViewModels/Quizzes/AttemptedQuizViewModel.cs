namespace QuizHut.Infrastructure.EntityViewModels.Quizzes
{
    using System.Collections.Generic;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Questions;

    public class AttemptedQuizViewModel : IMapFrom<Quiz>
    {
        public AttemptedQuizViewModel()
        {
            Questions = new List<AttemptedQuizQuestionViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Timer { get; set; }

        public string EventId { get; set; }

        public IList<AttemptedQuizQuestionViewModel> Questions { get; set; }
    }
}
