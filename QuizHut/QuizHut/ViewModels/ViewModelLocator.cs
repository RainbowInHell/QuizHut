namespace QuizHut.ViewModels
{
    using Microsoft.Extensions.DependencyInjection;

    class ViewModelLocator
    {
        public LoginViewModel LoginViewModel => App.Services.GetRequiredService<LoginViewModel>();
    }
}