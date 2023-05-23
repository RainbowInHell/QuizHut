namespace QuizHut.DLL.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            CreatedOn = DateTime.Now;
            StudentsInGroups = new HashSet<StudentGroup>();
            Students = new HashSet<ApplicationUser>();
            CreatedQuizzes = new HashSet<Quiz>();
            CreatedGroups = new HashSet<Group>();
            Results = new HashSet<Result>();
            CreatedEvents = new HashSet<Event>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? TeacherId { get; set; }

        public virtual ApplicationUser Teacher { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<StudentGroup> StudentsInGroups { get; set; }

        public virtual ICollection<ApplicationUser> Students { get; set; }

        public virtual ICollection<Quiz> CreatedQuizzes { get; set; }

        public virtual ICollection<Result> Results { get; set; }

        public virtual ICollection<Group> CreatedGroups { get; set; }

        public virtual ICollection<Event> CreatedEvents { get; set; }
    }
}