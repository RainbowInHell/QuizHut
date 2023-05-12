namespace QuizHut.DLL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using QuizHut.DLL.Entities;

    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> question)
        {
            question.HasMany(q => q.Answers)
                .WithOne(q => q.Question)
                .HasForeignKey(q => q.QuestionId);

            question.Property(q => q.Text)
                .HasMaxLength(1000)
                .IsRequired();
        }
    }
}