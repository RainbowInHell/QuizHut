namespace QuizHut.DLL.Entities
{
    public class EventGroup : BaseEntity<string>
    {
        public EventGroup()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string EventId { get; set; }

        public virtual Event Event { get; set; }

        public string GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}