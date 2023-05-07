namespace QuizHut.ViewModels.Factory
{
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;

    internal interface ISimpleTraderViewModelFactory
    {
        ViewModel CreateViewModel(ViewType viewType);
    }
}