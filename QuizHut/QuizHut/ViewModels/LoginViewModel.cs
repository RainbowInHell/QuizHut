using Microsoft.Xaml.Behaviors.Core;
using QuizHut.Services;
using QuizHut.Services.Contracts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ActionCommand = QuizHut.Infrastructure.Commands.ActionCommand;

namespace QuizHut.ViewModels
{
    class LoginViewModel
    {
        private string _username;
        private string _password;

        private readonly IAuthService _authService;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new ActionCommand(LoginAsync, CanLogin);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }

        private bool CanLogin(object parameter)
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        private async void LoginAsync(object parameter)
        {
            bool loginSuccessful = await _authService.LoginAsync(Username, Password);

            if (loginSuccessful)
            {
                MessageBox.Show("Good!");
            }
            else
            {
                MessageBox.Show("Bad!");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}