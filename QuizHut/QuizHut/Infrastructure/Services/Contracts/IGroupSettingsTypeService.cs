namespace QuizHut.Infrastructure.Services.Contracts
{
    using System;

    public enum GroupViewType
    {
        Create,
        Edit,
        AddStudents,
        AddEvents
    }

    public interface IGroupSettingsTypeService
    {
        GroupViewType? GroupViewType { get; set; }

        event Action StateChanged;
    }
}