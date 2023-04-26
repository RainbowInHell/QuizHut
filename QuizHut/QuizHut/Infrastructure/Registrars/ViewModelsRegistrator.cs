namespace QuizHut.Infrastructure.Registrars
{
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.ViewModels.LoginViewModels;
    using QuizHut.ViewModels.MainViewModels;

    public static class ViewModelsRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<AuthorizationViewModel>();
            services.AddSingleton<ResetPasswordViewModel>();
            services.AddSingleton<StudentRegistrationViewModel>();
            services.AddSingleton<TeacherRegistrationViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<UserProfileViewModel>();
            services.AddSingleton<ResultsViewModel>();
            services.AddSingleton<EventsViewModel>();
            services.AddSingleton<GroupsViewModel>();
            services.AddSingleton<CategoriesViewModel>();
            services.AddSingleton<QuizzesViewModel>();
            services.AddSingleton<StudentsViewModel>();

            return services;
        }
    }
}