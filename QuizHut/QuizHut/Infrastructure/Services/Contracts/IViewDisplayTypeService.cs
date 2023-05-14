namespace QuizHut.Infrastructure.Services.Contracts
{
    using System;

    public enum ViewDisplayType
    {
        Create,
        Edit,
        AddStudents,
        AddEvents,
        AddQuizzes
    }

    public interface IViewDisplayTypeService
    {
        ViewDisplayType? ViewDisplayType { get; set; }

        event Action StateChanged;
    }
}