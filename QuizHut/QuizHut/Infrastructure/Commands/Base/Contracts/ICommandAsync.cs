namespace QuizHut.Infrastructure.Commands.Base.Contracts
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    public interface ICommandAsync : ICommand
    {
        Task ExecuteAsync(object? parameter);
    }
}