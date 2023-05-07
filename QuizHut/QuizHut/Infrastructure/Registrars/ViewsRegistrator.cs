namespace QuizHut.Infrastructure.Registrars
{
    using Microsoft.Extensions.DependencyInjection;

    using QuizHut.ViewModels.MainViewModels;
    using QuizHut.Views.Windows;

    public static class ViewsRegistrator
    {
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddTransient(
                s =>
                {
                    var model = s.GetRequiredService<MainViewModel>();
                    var window = new MainView(model);
                    model.DialogComplete += (_, _) => window.Close();

                    return window;
                });

            return services;
        }
    }
}