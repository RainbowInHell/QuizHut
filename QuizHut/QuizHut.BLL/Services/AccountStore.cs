using QuizHut.BLL.Services.Contracts;
namespace QuizHut.BLL.Services
{
    public class AccountStore : IAccountStore
    {
        private bool isLoggedIn;
        public bool IsLoggedIn 
        { 
            get =>  isLoggedIn; 
            set
            {
                isLoggedIn = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}