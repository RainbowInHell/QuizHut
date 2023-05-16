namespace QuizHut.DLL.Entities
{
    public class Password : BaseEntity<int>
    {
        public string Content { get; set; }

        public string QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}