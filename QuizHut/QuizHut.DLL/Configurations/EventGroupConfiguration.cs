namespace QuizHut.DLL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using QuizHut.DLL.Entities;

    public class EventGroupConfiguration : IEntityTypeConfiguration<EventGroup>
    {
        public void Configure(EntityTypeBuilder<EventGroup> eventGroup)
        {
            eventGroup.HasKey(pg => new { pg.EventId, pg.GroupId });
        }
    }
}