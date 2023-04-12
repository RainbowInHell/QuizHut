namespace QuizHut.DAL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using QuizHut.DAL.Entities;

    public class StudentGroupConfiguration : IEntityTypeConfiguration<StudentGroup>
    {
        public void Configure(EntityTypeBuilder<StudentGroup> studentGroup)
        {
            studentGroup.HasKey(pg => new { pg.StudentId, pg.GroupId });
        }
    }
}