namespace QuizHut.DLL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using QuizHut.DLL.Entities;

    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> eventEntity)
        {
            eventEntity.HasOne(e => e.Quiz)
                .WithOne(q => q.Event)
                .HasForeignKey<Event>(e => e.QuizId);

            eventEntity.HasMany(e => e.EventsGroups)
                .WithOne(eg => eg.Event)
                .HasForeignKey(eg => eg.EventId);

            eventEntity.HasMany(e => e.Results)
                .WithOne(r => r.Event)
                .HasForeignKey(r => r.EventId);

            eventEntity.Property(g => g.Name)
           .HasMaxLength(50)
           .IsRequired();
        }
    }
}