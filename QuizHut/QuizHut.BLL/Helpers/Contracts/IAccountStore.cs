namespace QuizHut.BLL.Helpers.Contracts
{
    public interface IAccountStore
    {
        bool IsLoggedIn { get; set; }

        event Action StateChanged;
    }
}