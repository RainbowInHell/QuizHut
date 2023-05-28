using QuizHut.DLL.Entities;

namespace QuizHut.BLL.Helpers.Contracts
{
    public interface IAccountStore
    {
        ApplicationUser CurrentUser { get; set; }

        event Action StateChanged;
    }
}