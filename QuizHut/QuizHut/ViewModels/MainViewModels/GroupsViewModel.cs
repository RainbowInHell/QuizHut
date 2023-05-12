namespace QuizHut.ViewModels.MainViewModels
{
    using System.Windows.Input;

    using FontAwesome.Sharp;

    using QuizHut.Infrastructure.Commands;
    using QuizHut.Infrastructure.Services.Contracts;
    using QuizHut.ViewModels.Base;
    using QuizHut.ViewModels.Contracts;

    class GroupsViewModel : ViewModel, IMenuView
    {
        public static string Title { get; } = "Группы";
        public static IconChar IconChar { get; } = IconChar.PeopleGroup;

        private readonly IGroupSettingsTypeService groupSettingsTypeService;

        public GroupsViewModel(IRenavigator createGroupRenavigator, IGroupSettingsTypeService groupSettingsTypeService) 
        {
            this.groupSettingsTypeService = groupSettingsTypeService;

            NavigateCreateGroupCommand = new RenavigateCommand(createGroupRenavigator, GroupViewType.Create, groupSettingsTypeService);
            NavigateEditGroupCommand = new RenavigateCommand(createGroupRenavigator, GroupViewType.Edit, groupSettingsTypeService);
        }

        #region Commands

        public ICommand NavigateCreateGroupCommand { get; }
        public ICommand NavigateEditGroupCommand { get; }

        #endregion
    }
}