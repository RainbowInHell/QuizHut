namespace QuizHut.ViewModels
{
    using Microsoft.Extensions.DependencyInjection;

    class ViewModelLocator
    {
        public LoginViewModel LoginViewModel => App.Services.GetRequiredService<LoginViewModel>();

        public AuthorizationViewModel AuthorizationViewModel => App.Services.GetRequiredService<AuthorizationViewModel>();

        public ResetPasswordViewModel ResetPasswordViewModel => App.Services.GetRequiredService<ResetPasswordViewModel>();

        public StudentRegistrationViewModel StudentRegistrationViewModel => App.Services.GetRequiredService<StudentRegistrationViewModel>();

        public TeacherRegistrationViewModel TeacherRegistrationViewModel => App.Services.GetRequiredService<TeacherRegistrationViewModel>();

        public MainViewModel MainViewModel => App.Services.GetRequiredService<MainViewModel>();
    }
}