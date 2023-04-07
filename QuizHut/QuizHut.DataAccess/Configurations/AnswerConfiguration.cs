namespace QuizHut.DataAccess.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using QuizHut.DataAccess.Entities;

    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> answer)
        {
            answer.Property(a => a.Text)
                .HasMaxLength(1000)
                .IsRequired();

            answer.Property(a => a.IsRightAnswer)
                .HasDefaultValue(false);
        }
    }
}