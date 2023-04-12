namespace QuizHut.Infrastructure.Commands
{
    using QuizHut.Infrastructure.Commands.Base;
    using System.Windows;

    class MinimizeApplicationCommand : Command
    {
        public override bool CanExecute(object? parameter) => true;

        public override void Execute(object? parameter) => Application.Current.Windows[0].WindowState = WindowState.Minimized;
    }
}
