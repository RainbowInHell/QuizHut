namespace QuizHut.Infrastructure.Commands
{
    using System.Windows;

    using QuizHut.Infrastructure.Commands.Base;    

    class MaximizeApplicationCommand : Command
    {
        public override bool CanExecute(object? parameter) => true;

        public override void Execute(object? parameter)
        {
            if (Application.Current.Windows[0].WindowState == WindowState.Normal)
                Application.Current.Windows[0].WindowState = WindowState.Maximized;
            else
                Application.Current.Windows[0].WindowState = WindowState.Normal;
        }
    }
}