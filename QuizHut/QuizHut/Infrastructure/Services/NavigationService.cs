namespace QuizHut.Infrastructure.Services
{
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;
    using System;

    internal class NavigationService : ViewModel, INavigationService
    {
        private ViewModel currentView;
        private readonly Func<Type, ViewModel> _viewModelFactory;

        public ViewModel CurrentView
        {
            get => currentView;
            private set
            {
                Set(ref currentView, value);
            }
        }
        
        public NavigationService(Func<Type, ViewModel> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModel
        {
            ViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            if(viewModel is IResettable resettable)
            {
                resettable.Resert();
            }
            CurrentView = viewModel;
        }
    }
}