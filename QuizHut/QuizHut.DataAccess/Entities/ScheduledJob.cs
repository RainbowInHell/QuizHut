namespace QuizHut.DataAccess.Entities
{
    using QuizHut.DataAccess.Common.Models;

    public class ScheduledJob : BaseDeletableModel<int>
    {
        public string JobId { get; set; }

        public bool IsActivationJob { get; set; }

        public string EventId { get; set; }

        public virtual Event Event { get; set; }
    }
}