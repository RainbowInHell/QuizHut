namespace QuizHut.Infrastructure.Commands
{
    using QuizHut.Infrastructure.Commands.Base;
    using System.Windows;

    class CloseApplicationCommand : Command
    {
        public override bool CanExecute(object? parameter) => true;

        public override void Execute(object? parameter) => Application.Current.Shutdown();
    }
}
