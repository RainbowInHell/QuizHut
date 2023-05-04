namespace QuizHut.ViewModels.Base
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModel;

    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? PropertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;

            OnPropertyChanged(PropertyName);

            return true;
        }
    }
}