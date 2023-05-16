namespace QuizHut.DLL.Entities
{
    public class Group : BaseEntity<string>
    {
        public Group()
        {
            Id = Guid.NewGuid().ToString();
            StudentsGroups = new HashSet<StudentGroup>();
            EventsGroups = new HashSet<EventGroup>();
        }

        public string Name { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<StudentGroup> StudentsGroups { get; set; }

        public virtual ICollection<EventGroup> EventsGroups { get; set; }
    }
}