namespace QuizHut.BLL.Services.Contracts
{
    public interface IAccountStore
    {
        bool IsLoggedIn { get; set; }

        event Action StateChanged;
    }
}