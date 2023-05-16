namespace QuizHut.DLL.Entities
{
    public class ScheduledJob : BaseEntity<string>
    {
        public ScheduledJob()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string JobId { get; set; }

        public bool IsActivationJob { get; set; }

        public string EventId { get; set; }

        public virtual Event Event { get; set; }
    }
}