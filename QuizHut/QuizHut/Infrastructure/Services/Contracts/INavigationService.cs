namespace QuizHut.Infrastructure.Services.Contracts
{
    using System;

    using QuizHut.ViewModels.Base;
    

    public enum ViewType
    {
        Authorization,
        StudentRegistration,
        TeacherRegistration,
        ResetPassword,

        Home,

        Category,
        CategoryActions,
        CategorySettings,

        Event,
        EventActions,
        EventSettings,

        Group,
        GroupActions,
        GroupSettings,

        Quiz,
        AddEditQuiz,

        Result,
        Student,
        UserProfile,
    }

    internal interface INavigationService
    {
        ViewModel CurrentView { get; set; }

        event Action StateChanged;
    }
}