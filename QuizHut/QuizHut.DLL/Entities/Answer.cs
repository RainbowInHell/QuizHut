namespace QuizHut.DLL.Entities
{
    public class Answer : BaseEntity<string>
    {
        public Answer()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Text { get; set; }

        public bool IsRightAnswer { get; set; }

        public string QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}