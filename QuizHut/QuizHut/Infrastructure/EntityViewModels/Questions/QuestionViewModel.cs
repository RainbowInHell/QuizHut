namespace QuizHut.Infrastructure.EntityViewModels.Questions
{
    using System.Collections.Generic;

    using QuizHut.BLL.MapperConfig.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.EntityViewModels.Answers;

    public class QuestionViewModel : IMapFrom<Question>
    {
        public QuestionViewModel()
        {
            Answers = new List<AnswerViewModel>();
        }

        public string Id { get; set; }

        public string Text { get; set; }
        
        public int Number { get; set; }

        public bool IsFullEvaluation { get; set; }

        public IList<AnswerViewModel> Answers { get; set; }
    }
}