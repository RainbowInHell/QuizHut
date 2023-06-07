namespace QuizHut.DLL.EntityFramework
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using QuizHut.DLL.Entities;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Result> Results { get; set; }

        public DbSet<StudentGroup> StudentsGroups { get; set; }

        public DbSet<EventGroup> EventsGroups { get; set; }

        public DbSet<ScheduledJob> ScheduledJobs { get; set; }
    }
}