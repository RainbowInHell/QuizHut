namespace QuizHut.DLL.Entities
{
    public class Category : BaseEntity<string>
    {
        public Category()
        {
            Id = Guid.NewGuid().ToString();
            Quizzes = new HashSet<Quiz>();
        }

        public string Name { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<Quiz> Quizzes { get; set; }
    }
}