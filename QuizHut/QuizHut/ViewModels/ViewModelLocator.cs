namespace QuizHut.ViewModels
{
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.ViewModels.LoginViewModels;
    using QuizHut.ViewModels.MainViewModels;

    class ViewModelLocator
    {
        public LoginViewModel LoginViewModel => App.Services.GetRequiredService<LoginViewModel>();

        public AuthorizationViewModel AuthorizationViewModel => App.Services.GetRequiredService<AuthorizationViewModel>();

        public ResetPasswordViewModel ResetPasswordViewModel => App.Services.GetRequiredService<ResetPasswordViewModel>();

        public StudentRegistrationViewModel StudentRegistrationViewModel => App.Services.GetRequiredService<StudentRegistrationViewModel>();

        public TeacherRegistrationViewModel TeacherRegistrationViewModel => App.Services.GetRequiredService<TeacherRegistrationViewModel>();

        public MainViewModel MainViewModel => App.Services.GetRequiredService<MainViewModel>();

        public HomeViewModel HomeViewModel => App.Services.GetRequiredService<HomeViewModel>();

        public UserProfileViewModel UserProfileViewModel => App.Services.GetRequiredService<UserProfileViewModel>();

        public ResultsViewModel ResultsViewModel => App.Services.GetRequiredService<ResultsViewModel>();

        public EventsViewModel EventsViewModel => App.Services.GetRequiredService<EventsViewModel>();

        public GroupsViewModel GroupsViewModel => App.Services.GetRequiredService<GroupsViewModel>();

        public CategoriesViewModel CategoriesViewModel => App.Services.GetRequiredService<CategoriesViewModel>();

        public QuizzesViewModel QuizzesViewModel => App.Services.GetRequiredService<QuizzesViewModel>();

        public StudentsViewModel StudentsViewModel => App.Services.GetRequiredService<StudentsViewModel>();
    }
}