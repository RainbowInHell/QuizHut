namespace QuizHut.Infrastructure.Services.Contracts
{
    using System;

    public enum ViewDisplayType
    {
        Create,
        Edit,
        AddStudents,
        AddEvents,
        AddQuizzes,
        AddGroups,
    }

    public interface IViewDisplayTypeService
    {
        ViewDisplayType? ViewDisplayType { get; set; }

        event Action StateChanged;
    }
}