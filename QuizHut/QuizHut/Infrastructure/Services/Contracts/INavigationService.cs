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
        Event,

        Group,
        GroupActions,
        GroupSettings,

        Quiz,
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