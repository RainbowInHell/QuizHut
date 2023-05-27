namespace QuizHut.ViewModels.MainViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Commands.Base;
    using QuizHut.Infrastructure.Commands.Base.Contracts;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class UserProfileViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Профиль"; 

        public static IconChar IconChar { get; } = IconChar.AddressCard;

        private readonly IUserAccountService userAccountService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IRenavigator authorizationRenavigator;

        public UserProfileViewModel(
            IUserAccountService userAccountService,
            ISharedDataStore sharedDataStore,
            IRenavigator authorizationRenavigator)
        {
            this.userAccountService = userAccountService;
            this.sharedDataStore = sharedDataStore;
            this.authorizationRenavigator = authorizationRenavigator;

            LoadDataCommand = new ActionCommand(OnLoadDataCommandExecuted);
            DeleteUserCommandAsync = new ActionCommandAsync(OnDeleteUserCommandAsyncExecute);
        }

        #region FieldsAndProperties

        private ApplicationUser currentUser;
        public ApplicationUser CurrentUser
        {
            get => currentUser;
            set => Set(ref currentUser, value);
        }

        #endregion

        #region LoadDataCommand

        public ICommand LoadDataCommand { get; }

        private void OnLoadDataCommandExecuted(object p)
        {
            LoadUserDataAsync();
        }

        #endregion

        #region DeleteUserCommandAsync

        public ICommandAsync DeleteUserCommandAsync { get; }

        private async Task OnDeleteUserCommandAsyncExecute(object p)
        {
            //var result = await userAccountService.DeleteUserAsync(CurrentUser.Id);

            //if (result.Succeeded)
            //{
            //    userAccountService.Logout();

            //    authorizationRenavigator.Renavigate();
            //}

            userAccountService.Logout();

            authorizationRenavigator.Renavigate();
        }

        #endregion

        private void LoadUserDataAsync()
        {
            CurrentUser = new ApplicationUser()
            {
                Id = sharedDataStore.CurrentUser.Id,
                Email = sharedDataStore.CurrentUser.Email,
                FirstName = sharedDataStore.CurrentUser.FirstName,
                LastName = sharedDataStore.CurrentUser.LastName
            };
        }
    }
}