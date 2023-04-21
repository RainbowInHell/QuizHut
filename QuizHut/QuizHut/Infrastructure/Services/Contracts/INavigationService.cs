namespace QuizHut.Infrastructure.Services.Contracts
{
    using System;

    using QuizHut.ViewModels.Base;
    
    internal interface INavigationService
    {
        ViewModel CurrentView { get; }

        void NavigateTo(Type viewModelType);
        void NavigateTo<T>() where T : ViewModel;
    }
}