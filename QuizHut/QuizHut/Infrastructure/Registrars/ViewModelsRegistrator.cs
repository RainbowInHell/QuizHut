namespace QuizHut.Infrastructure.Registrars
{
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.ViewModels;

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

            return services;
        }
    }
}