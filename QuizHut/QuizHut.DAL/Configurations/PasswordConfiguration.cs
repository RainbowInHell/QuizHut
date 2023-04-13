namespace QuizHut.DAL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using QuizHut.DAL.Entities;

    public class PasswordConfiguration : IEntityTypeConfiguration<Password>
    {
        public void Configure(EntityTypeBuilder<Password> passsword)
        {
            passsword.HasOne(p => p.Quiz)
                .WithOne(q => q.Password)
                .HasForeignKey<Password>(p => p.QuizId);

            passsword.Property(q => q.Content)
               .HasMaxLength(16)
               .IsRequired();

            passsword.HasIndex(x => x.Content)
                .IsUnique();
        }
    }
}