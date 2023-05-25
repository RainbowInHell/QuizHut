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
        ViewDisplayType? CurrentViewDisplayType { get; set; }

        event Action StateChanged;
    }
}