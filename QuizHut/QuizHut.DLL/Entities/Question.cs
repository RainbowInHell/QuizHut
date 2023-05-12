namespace QuizHut.DLL.Entities
{
    using QuizHut.DLL.Common.Models;

    public class Question : BaseDeletableModel<string>
    {
        public Question()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Answers = new HashSet<Answer>();
        }

        public string Text { get; set; }

        public int Number { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public string QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}