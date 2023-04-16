namespace QuizHut.Infrastructure.Commands
{
    using System.Windows;

    using QuizHut.Infrastructure.Commands.Base;

    class CloseApplicationCommand : Command
    {
        public override bool CanExecute(object? parameter) => true;

        public override void Execute(object? parameter) => Application.Current.Shutdown();
    }
}