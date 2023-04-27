namespace QuizHut.Infrastructure.Commands.Base
{
    using System;
    using System.Threading.Tasks;

    internal class ActionCommandAsync : CommandAsync
    {
        private readonly Func<object, Task> executeAsync;

        private readonly Func<object, bool> canExecute;

        public ActionCommandAsync(Func<object, Task> executeAsync, Func<object, bool> canExecute = null)
        {
            this.executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            this.canExecute = canExecute;
        }

        public override bool CanExecute(object? parameter) => canExecute?.Invoke(parameter) ?? true;

        public override async Task ExecuteAsync(object? parameter) => await executeAsync(parameter);
    }
}