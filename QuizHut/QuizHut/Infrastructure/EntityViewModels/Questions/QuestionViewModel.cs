using QuizHut.BLL.MapperConfig.Contracts;
using QuizHut.DLL.Entities;
using System;
namespace QuizHut.Infrastructure.EntityViewModels.Questions
{
    using QuizHut.Infrastructure.EntityViewModels.Answers;
    using System.Collections.Generic;

    public class QuestionViewModel : IMapFrom<Question>
    {
        public QuestionViewModel()
        {
            Answers = new List<AnswerViewModel>();
        }

        public string Id { get; set; }

        public string Text { get; set; }

        //public string SanitizedText => new HtmlSanitizer().Sanitize(this.Text);

        public IList<AnswerViewModel> Answers { get; set; }

        public int Number { get; set; }
    }
}