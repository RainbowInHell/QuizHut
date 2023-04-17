namespace QuizHut.ViewModels
{
    using Microsoft.Extensions.DependencyInjection;

    class ViewModelLocator
    {
        public LoginViewModel LoginViewModel => App.Services.GetRequiredService<LoginViewModel>();

        public AuthorizationViewModel AuthorizationViewModel => App.Services.GetRequiredService<AuthorizationViewModel>();

        public ResetPasswordViewModel ResetPasswordViewModel => App.Services.GetRequiredService<ResetPasswordViewModel>();
    }
}