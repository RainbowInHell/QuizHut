namespace QuizHut.ViewModels
{
    using QuizHut.Infrastructure.Commands;
    using System;
    using System.Windows;
    using System.Windows.Input;

    class LoginViewModel
    {
        #region Commands

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; set; }
        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p) { Application.Current.Shutdown(); }

        #endregion

        public ICommand MinimizeApplicationCommand { get; set; }
        private bool CanMinimizeApplicationCommandExecute(object p) => true;
        private void OnMinimizeApplicationCommandExecuted(object p) { Application.Current.Windows[0].WindowState = WindowState.Minimized; }

        #endregion

        public LoginViewModel() 
        {
            #region Commands

            CloseApplicationCommand = new ActionCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            MinimizeApplicationCommand = new ActionCommand(OnMinimizeApplicationCommandExecuted, CanMinimizeApplicationCommandExecute);

            #endregion
        }
    }
}
