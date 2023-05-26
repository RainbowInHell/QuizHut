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
        StudentHome,

        Category,
        CategoryActions,
        CategorySettings,

        Event,
        EventActions,
        EventSettings,

        StudentActiveEvents,
        StudentPendingEvents,
        StudentEndedEvents,

        Group,
        GroupActions,
        GroupSettings,

        Quiz,
        AddEditQuiz,
        AddEditQuestion,
        QuizSettings,
        AddEditAnswer,
        StartQuiz,
        TakingQuiz,
        EndQuiz,

        Result,
        ActiveResults,
        EndedResults,
        ResultsForEvent,

        OwnResult,

        Student,
        UserProfile,
    }

    internal interface INavigationService
    {
        ViewModel CurrentView { get; set; }

        event Action StateChanged;
    }
}