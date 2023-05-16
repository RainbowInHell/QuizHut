namespace QuizHut.DLL.Entities
{
    public class StudentGroup : BaseEntity<string>
    {
        public StudentGroup()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string StudentId { get; set; }

        public virtual ApplicationUser Student { get; set; }

        public string GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}