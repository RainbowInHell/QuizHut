namespace QuizHut.ViewModels
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ViewModelRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
            .AddSingleton<LoginViewModel>()
            .AddSingleton<AuthorizationViewModel>()
            ;
    }
}