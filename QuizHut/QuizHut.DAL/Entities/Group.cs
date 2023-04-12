using QuizHut.DAL.Common.Models;

namespace QuizHut.DAL.Entities
{
    public class Group : BaseDeletableModel<string>
    {
        public Group()
        {
            this.Id = Guid.NewGuid().ToString();
            this.StudentsGroups = new HashSet<StudentGroup>();
            this.EventsGroups = new HashSet<EventGroup>();
        }

        public string Name { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<StudentGroup> StudentsGroups { get; set; }

        public virtual ICollection<EventGroup> EventsGroups { get; set; }
    }
}