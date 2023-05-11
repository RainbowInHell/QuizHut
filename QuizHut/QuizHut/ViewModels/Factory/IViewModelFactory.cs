namespace QuizHut.ViewModels.Factory
{
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    internal interface IViewModelFactory
    {
        ViewModel CreateViewModel(ViewType viewType);
    }
}