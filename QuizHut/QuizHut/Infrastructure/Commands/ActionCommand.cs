namespace QuizHut.Infrastructure.Commands
{
    using System;

    using QuizHut.Infrastructure.Commands.Base;

    internal class ActionCommand : Command
    {
        private readonly Action<object> execute;

        private readonly Func<object, bool> canExecute;

        public ActionCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            canExecute = CanExecute;
        }

        public override bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object? parameter) => execute(parameter);
    }
}