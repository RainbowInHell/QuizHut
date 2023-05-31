namespace QuizHut.DLL.Entities
{
    public class Result : BaseEntity<string>
    {
        public Result()
        {
            Id = Guid.NewGuid().ToString();
        }

        public decimal Points { get; set; } = 0;

        public int? MaxPoints { get; set; }

        public TimeSpan TimeSpent { get; set; } = TimeSpan.Zero;

        public string StudentId { get; set; }

        public virtual ApplicationUser Student { get; set; }

        public string QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}