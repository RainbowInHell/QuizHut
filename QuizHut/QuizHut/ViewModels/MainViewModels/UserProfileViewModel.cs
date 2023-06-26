namespace QuizHut.ViewModels.MainViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.BLL.Helpers.Contracts;
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
        public string Title { get; set; } = "Профиль"; 

        public IconChar IconChar { get; set; } = IconChar.AddressCard;

        private readonly IUserAccountService userAccountService;

        private readonly ISharedDataStore sharedDataStore;

        private readonly IAccountStore accountStore;

        private readonly IExporter exporter;

        private readonly IRenavigator authorizationRenavigator;

        public UserProfileViewModel(
            IUserAccountService userAccountService,
            ISharedDataStore sharedDataStore,
            IAccountStore accountStore,
            IExporter exporter,
            IRenavigator authorizationRenavigator)
        {
            this.userAccountService = userAccountService;
            this.sharedDataStore = sharedDataStore;
            this.accountStore = accountStore;
            this.exporter = exporter;
            this.authorizationRenavigator = authorizationRenavigator;

            LoadDataCommand = new ActionCommand(OnLoadDataCommandExecuted);
            DeleteUserCommandAsync = new ActionCommandAsync(OnDeleteUserCommandAsyncExecute);
            UpdateUserCommandAsync = new ActionCommandAsync(OnUpdateUserCommandAsyncExecute, CanUpdateUserCommandAsyncExecute);
            ExportFirstDataCommandAsync = new ActionCommandAsync(OnExportFirstDataCommandAsyncExecute);
            ExportSecondDataCommandAsync = new ActionCommandAsync(OnExportSecondDataCommandAsyncExecute);
            ExportThirdDataCommandAsync = new ActionCommandAsync(OnExportThirdDataCommandAsyncExecute);
            ExportFourthDataCommandAsync = new ActionCommandAsync(OnExportFourthDataCommandAsyncExecute);
            ExportFifthDataCommandAsync = new ActionCommandAsync(OnExportFifthDataCommandAsyncExecute);
            ExportSixthDataCommandAsync = new ActionCommandAsync(OnExportSixthDataCommandAsyncExecute);
            ExportSeventhDataCommandAsync = new ActionCommandAsync(OnExportSeventhDataCommandAsyncExecute);
        }

        #region FieldsAndProperties

        public UserRole CurrentUserRole => accountStore.CurrentUserRole;

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

        #region UpdateUserCommandAsync

        public ICommandAsync UpdateUserCommandAsync { get; }

        private bool CanUpdateUserCommandAsyncExecute(object p)
        {
            if (CurrentUser == null || string.IsNullOrEmpty(CurrentUser.FirstName) || string.IsNullOrEmpty(CurrentUser.FirstName)) 
            {
                return false;
            }

            return true;
        }

        private async Task OnUpdateUserCommandAsyncExecute(object p)
        {
            var result = await userAccountService.UpdateUserAsync(CurrentUser);

            if (result != null)
            {
                CurrentUser = result;
                //LoadUserDataAsync();
            }
        }

        #endregion

        #region DeleteUserCommandAsync

        public ICommandAsync DeleteUserCommandAsync { get; }

        private async Task OnDeleteUserCommandAsyncExecute(object p)
        {
            var result = await userAccountService.DeleteUserAsync(CurrentUser.Id);

            if (result.Succeeded)
            {
                userAccountService.Logout();

                authorizationRenavigator.Renavigate();
            }
        }

        #endregion

        #region ExportFirstDataCommand

        public ICommandAsync ExportFirstDataCommandAsync { get; }

        private async Task OnExportFirstDataCommandAsyncExecute(object p)
        {
            await exporter.GenerateEventParticipationReportAsync();
        }

        #endregion

        #region ExportSecondDataCommand

        public ICommandAsync ExportSecondDataCommandAsync { get; }

        private async Task OnExportSecondDataCommandAsyncExecute(object p)
        {
            await exporter.GenerateGroupPerformanceReportAsync();
        }

        #endregion

        #region ExportThirdDataCommand

        public ICommandAsync ExportThirdDataCommandAsync { get; }

        private async Task OnExportThirdDataCommandAsyncExecute(object p)
        {
            await exporter.GenerateQuizPerformanceReportByCategoryAsync();
        }

        #endregion

        #region ExportFourthDataCommand

        public ICommandAsync ExportFourthDataCommandAsync { get; }

        private async Task OnExportFourthDataCommandAsyncExecute(object p)
        {
            await exporter.GenerateTopPerformingStudentsByQuizAsync();
        }

        #endregion

        #region ExportFifthDataCommand

        public ICommandAsync ExportFifthDataCommandAsync { get; }

        private async Task OnExportFifthDataCommandAsyncExecute(object p)
        {
            await exporter.GenerateQuizCompletionRateByGroupAsync();
        }

        #endregion

        #region ExportSixthDataCommand

        public ICommandAsync ExportSixthDataCommandAsync { get; }

        private async Task OnExportSixthDataCommandAsyncExecute(object p)
        {
            await exporter.GenerateStudentPerformanceDistributionByCategoryAsync();
        }

        #endregion

        #region ExportSeventhDataCommand

        public ICommandAsync ExportSeventhDataCommandAsync { get; }

        private async Task OnExportSeventhDataCommandAsyncExecute(object p)
        {
            await exporter.GenerateTimeSpentOnQuizzesByStudentAsync();
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