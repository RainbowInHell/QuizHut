namespace QuizHut.Infrastructure.Commands.Base.Contracts
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    public interface ICommandAsyn : ICommand
    {
        Task ExecuteAsync(object? parameter);
    }
}