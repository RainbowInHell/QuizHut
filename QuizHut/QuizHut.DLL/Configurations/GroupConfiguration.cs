namespace QuizHut.DLL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
    using QuizHut.DLL.Entities;

    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> group)
        {
            group.HasMany(g => g.StudentsGroups)
              .WithOne(p => p.Group)
              .HasForeignKey(q => q.GroupId);

            group.HasMany(g => g.EventsGroups)
             .WithOne(p => p.Group)
             .HasForeignKey(q => q.GroupId);

            group.Property(g => g.Name)
             .HasMaxLength(50)
             .IsRequired();
        }
    }
}