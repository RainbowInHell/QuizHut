namespace QuizHut.DLL.Entities
{
    public class Quiz : BaseEntity<string>
    {
        public Quiz()
        {
            Id = Guid.NewGuid().ToString();
            Questions = new HashSet<Question>();
            Results = new HashSet<Result>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? Timer { get; set; }

        public string CreatorId { get; set; }

        public string Password { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public string? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public string? EventId { get; set; }

        public virtual Event Event { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<Result> Results { get; set; }
    }
}