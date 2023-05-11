namespace QuizHut.DLL.Entities
{
    using QuizHut.DLL.Common.Models;

    public class Category : BaseDeletableModel<string>
    {
        public Category()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Quizzes = new HashSet<Quiz>();
        }

        public string Name { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<Quiz> Quizzes { get; set; }
    }
}