namespace QuizHut.DLL.Entities
{
    using QuizHut.DLL.Common.Models;

    public class EventGroup : BaseDeletableModel<string>
    {
        public string EventId { get; set; }

        public virtual Event Event { get; set; }

        public string GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}