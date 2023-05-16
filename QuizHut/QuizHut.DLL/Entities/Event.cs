namespace QuizHut.DLL.Entities
{
    using QuizHut.DLL.Common;

    public class Event : BaseEntity<string>
    {
        public Event()
        {
            Id = Guid.NewGuid().ToString();
            Results = new HashSet<Result>();
            EventsGroups = new HashSet<EventGroup>();
            ScheduledJobs = new HashSet<ScheduledJob>();
        }

        public string Name { get; set; }

        public string CreatorId { get; set; }

        public Status Status { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public DateTime ActivationDateAndTime { get; set; }

        public TimeSpan DurationOfActivity { get; set; }

        public string? QuizId { get; set; }

        public string? QuizName { get; set; }

        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<Result> Results { get; set; }

        public virtual ICollection<EventGroup> EventsGroups { get; set; }

        public virtual ICollection<ScheduledJob> ScheduledJobs { get; set; }
    }
}