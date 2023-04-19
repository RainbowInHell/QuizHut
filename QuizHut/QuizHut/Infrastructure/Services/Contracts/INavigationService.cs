namespace QuizHut.Infrastructure.Services.Contracts
{
    using QuizHut.ViewModels.Base;

    internal interface INavigationService
    {
        ViewModel CurrentView { get; }

        void NavigateTo<T>() where T : ViewModel;
    }
}