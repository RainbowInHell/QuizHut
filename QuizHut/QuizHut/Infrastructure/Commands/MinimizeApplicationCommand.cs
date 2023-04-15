namespace QuizHut.Infrastructure.Commands
{
    using System.Threading.Tasks;
    using System.Windows;

    using QuizHut.Infrastructure.Commands.Base;

    class MinimizeApplicationCommand : Command
    {
        public override bool CanExecute(object? parameter) => true;

        public override void Execute(object? parameter) => Application.Current.Windows[0].WindowState = WindowState.Minimized;
    }
}