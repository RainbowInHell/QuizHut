namespace QuizHut.DLL.Entities
{
    public class Question : BaseEntity<string>
    {
        public Question()
        {
            Id = Guid.NewGuid().ToString();
            Answers = new HashSet<Answer>();
        }

        public string Text { get; set; }

        public int Number { get; set; }

        public bool IsFullEvaluation { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public string QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}