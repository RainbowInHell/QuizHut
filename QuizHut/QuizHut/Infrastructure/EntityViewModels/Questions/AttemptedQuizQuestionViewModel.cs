namespace QuizHut.Infrastructure.EntityViewModels.Questions
{
    using System.Collections.Generic;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Answers;

    public class AttemptedQuizQuestionViewModel : IMapFrom<Question>
    {
        public AttemptedQuizQuestionViewModel()
        {
            Answers = new List<AttemptedQuizAnswerViewModel>();
        }

        public string Id { get; set; }

        public string Text { get; set; }

        public int Number { get; set; }
        
        public bool IsFullEvaluation { get; set; }

        public IList<AttemptedQuizAnswerViewModel> Answers { get; set; }
    }
}